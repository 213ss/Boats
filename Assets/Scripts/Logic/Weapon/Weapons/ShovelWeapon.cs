using System;
using System.Collections;
using Actors;
using Damageable;
using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Infrastructure.Services.VFXGoldBurst;
using Infrastructure.Services.Vibrate;
using Logic.Triggers;
using Scripts.Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace Logic.Weapon.Weapons
{
    public class ShovelWeapon : Weapon, IShovelWeapon
    {
        public event Action OnShovelStrike;
        public event Action OnExcavated;
        
        public bool IsStayInGoldArea { get; set; }
        public bool IsTriggerStay { get; set; }
        public bool IsExcavated { get; private set; }

        private float _goldPrize;
        

        [Header("Shovel strike parameters")]
        [SerializeField] private float _forwardPower;
        [SerializeField] private float _upPower;
        
        [Header("Excavation parameters")]
        [SerializeField] private float _excavationTime;

        [Header("VFX")]
        [SerializeField] private VFXGoldBurst _vfxGoldBurst;
        
        [Header("Shovel hole parameters")]
        [SerializeField] private float _holeDistanceOffset;
        [SerializeField] private float _holeDistanceFromGround;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _groundDirection;

        private bool _isDetectEnemy;
        
        private IActorDetector _actorDetector;
        private Actor _ownerActor;
        private Actor _enemyActor;

        private IGameFactory _gameFactory;

        private GoldAreaTrigger _goldArea;

        private StickmanAnimator _stickmanAnimator;
        private IVibrate _vibrate;
        
        private Coroutine _excavationCoroutine;
        private Coroutine _strikeCoroutine;
        
        
        [Inject]
        public void Construct(IActorDetector actorDetector, IGameFactory gameFactory, IVibrate vibrate)
        {
            _actorDetector = actorDetector;
            _actorDetector.OnDetectActor += DetectActor;
            _vibrate = vibrate;
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _stickmanAnimator = GetComponentInChildren<StickmanAnimator>();
            _ownerActor = GetComponentInParent<Actor>();
            _actorDetector.EnableDetect();
            
            _vfxGoldBurst.OnMoveToTargetEnd += AddGoldToActor;
        }

        public float GetHolDistance()
        {
            return _holeDistanceOffset;
        }

        public void SetGoldExcavation(float goldPrize)
        {
            _goldPrize = goldPrize;
        }

        public override void UseWeapons()
        {
        }

        public override void WeaponOnMouseUp()
        {
            if (_excavationCoroutine != null)
            {
                _stickmanAnimator.StopDigging();
                StopCoroutine(_excavationCoroutine);
                _excavationCoroutine = null;
                IsExcavated = false;
            }
        }

        public void Excavate()
        {
            if (_excavationCoroutine == null)
            {
                _excavationCoroutine = StartCoroutine(Excavation());
                IsExcavated = true;
            }
        }

        public void Attack()
        {
            if (_strikeCoroutine == null)
            {
                if(_ownerActor.ActorTeam == Team.Player_0)
                    //_vibrate.PlayVibrate(0.4f);
                
                _strikeCoroutine = StartCoroutine(ShovelStrike());
            }
        }

        public void SetGoldTrigger(GoldAreaTrigger trigger)
        {
            _goldArea = trigger;
        }

        private IEnumerator ShovelStrike()
        {
            _stickmanAnimator.OnHit();
            
            if (_isDetectEnemy == false)
            {
                _strikeCoroutine = null;
                yield break;
            }

            float waitedHitTimer = 4.0f;
            while (_stickmanAnimator.IsHit == false)
            {
                if (waitedHitTimer <= 0.0f)
                {
                    _strikeCoroutine = null;
                    yield break;
                }

                waitedHitTimer -= Time.deltaTime;
                yield return null;
            }

            if (_isDetectEnemy == false)
            {
                _strikeCoroutine = null;
                yield break;
            }
            
            ApplyForceToEnemy();
            _strikeCoroutine = null;
        }

        private void ApplyForceToEnemy()
        {
            if (_enemyActor != null)
            {
                if (_enemyActor.TryGetComponent<IApplyForce>(out var applyForce))
                {
                    Vector3 forceVector =
                        (_enemyActor.ActorTransform.position - _ownerActor.ActorTransform.position);

                    var force = forceVector.normalized;

                    force *= _forwardPower;
                    force.y = _upPower;

                    applyForce.ApplyForce(force);
                }
            }

            OnShovelStrike?.Invoke();
        }

        private IEnumerator Excavation()
        {
            if (IsExcavateOnGround() == false) yield break;
            
            _stickmanAnimator.StartDigging();
            
            float currentTime = _excavationTime;

            while (currentTime >= 0.0f)
            {
                currentTime -= Time.deltaTime;
                yield return null;
            }
            
            //if(_ownerActor.ActorTeam == Team.Player_0)
                //_vibrate.PlayVibrate(0.4f);
            
            _stickmanAnimator.StopDigging();

            var ownerPosition = _ownerActor.ActorTransform.position;
            ownerPosition.y = _holeDistanceFromGround;
            
            var holePosition = ownerPosition + (_ownerActor.ActorTransform.forward * _holeDistanceOffset);
            
            CreateHoleAt(holePosition);

            if (IsStayInGoldArea)
            {
                _vfxGoldBurst.PlayGoldVFX(holePosition, Mathf.RoundToInt(_goldPrize), _ownerActor);
                IsStayInGoldArea = false;
            }

            IsExcavated = false;
            OnExcavated?.Invoke();

            _excavationCoroutine = null;
        }

        private void AddGoldToActor()
        {
            if (_goldArea != null)
            {
                _goldArea.CloseTrigger();
                _goldArea = null;
            }

            _ownerActor.GoldService.AddGoldDelay(_goldPrize, Mathf.RoundToInt(_goldPrize), 0.02f);
            _goldPrize = 0.0f;
        }

        private void CreateHoleAt(Vector3 position)
        {
            Vector3 spawnPosition = position;
            _gameFactory.CreateGameObject(AssetsPath.HoleTrigger, spawnPosition);
        }

        private void DetectActor(Actor actor)
        {
            if (actor != null)
            {
                if (actor != null && _ownerActor.ActorTeam != actor.ActorTeam)
                {
                    _enemyActor = actor;
                    _isDetectEnemy = true;
                    return;
                }
            }

            _enemyActor = null;
        }

        private bool IsExcavateOnGround()
        {
            Ray ray = new Ray(_ownerActor.ActorTransform.position, _groundDirection.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 25.0f, _groundLayer))
            {
                return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            _actorDetector.OnDetectActor -= DetectActor;
            _vfxGoldBurst.OnMoveToTargetEnd -= AddGoldToActor;
        }

        private void OnDisable()
        {
            _actorDetector.OnDetectActor -= DetectActor;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _groundDirection.forward * 25.0f);
        }

#endif
    }
}