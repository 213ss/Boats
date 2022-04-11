using System.Collections;
using Damageable;
using Infrastructure.Data;
using Infrastructure.Services.Girls;
using Infrastructure.Services.GoldLoot;
using Infrastructure.Services.GroundDetector;
using Infrastructure.Services.InputServices;
using Infrastructure.Services.SkinChanger;
using Infrastructure.Services.Vibrate;
using Logic.Decal;
using Logic.ImmortalAnim;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Actors.Actors
{
    public class PlayerActor : Actor, IApplyForce, ISavedProgress
    {
        [SerializeField] private float _immortalTime;
        [SerializeField] private RadiusDecal _decal;
        
        private IInputServices _inputServices;
        private IGroundDetector _groundDetector;
        
        [Inject(Id = "ActorMove")] private IMovement _movement;

        private bool _isGrounded;
        private Rigidbody _rigidbody;
        private bool _isActive;
        private bool _isAttacked;

        private StickmanAnimator _stickmanAnimator;
        private IVibrate _vibrate;
        private IGoldLootService _goldLootService;
        private ISkin _skinService;

        private ImmortalAnimation _immortalAnimation;
        private Coroutine _immortalTimeCoroutine;
        private IGirlsService _girlService;

        private bool _goToFinalPoint;


        [Inject]
        private void Construct(IInputServices inputServices, 
            IGroundDetector groundDetector, 
            IVibrate vibrate, 
            IGoldLootService goldLootService, ISkin skinService, IGirlsService girlsService)
        {
            _inputServices = inputServices;
            _groundDetector = groundDetector;
            _vibrate = vibrate;
            _groundDetector.OnGrounded += OnGrounded;
            _goldLootService = goldLootService;
            _skinService = skinService;
            _girlService = girlsService;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.isKinematic = true;
                _rigidbody.freezeRotation = true;
            }
            
            _stickmanAnimator = GetComponentInChildren<StickmanAnimator>();
            _groundDetector.EnableGroundChecker();

            _immortalAnimation = GetComponent<ImmortalAnimation>();
        }

        private void Update()
        {
            if (IsWin) return;
            if (IsDroppedOutGame) return;
            if(_goToFinalPoint) return;
            
        
            if(_isActive == false) return;
            
            if (_inputServices.IsLeftMouseButton())
            {
                _movement.Move(_inputServices.Axes());
            }
            else
            {
                _stickmanAnimator.StopMoving();
            }
            
        }

        public void GoToFinalPoint()
        {
            _goToFinalPoint = true;
            _stickmanAnimator.Move();
        }

        public override void EnableActor()
        {
            _movement.EnableMoveComponent();
            _isActive = true;
        }

        public override void DisableActor()
        {
            _movement.DisableMoveComponent();
            _isActive = false;
        }

        public override void DroppedOutGame()
        {
            base.DroppedOutGame();

            _groundDetector.DisableGroundChecker();
            _movement.DisableMoveComponent();
        }


        public void ApplyForce(Vector3 force)
        {
            if(IsTravel) return;
            if(_isGrounded == false) return;
            if(_isAttacked) return;

            if(_immortalTimeCoroutine != null)
                return;
                       
            
            _immortalTimeCoroutine = StartCoroutine(ImmortalState());
            
            _isAttacked = true;
            IsAttacked = true;

            _goldLootService.OnDropGold(GoldService, ActorTransform.position, this);
            
            
            //_vibrate.PlayVibrate(0.4f);
            _movement.DisableMoveComponent();
            _rigidbody.isKinematic = false;

            _rigidbody.AddForce(force, ForceMode.Impulse);
            _groundDetector.EnableGroundChecker();
        }

        private void OnGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;

            if (isGrounded && IsDroppedOutGame == false)
            {
                _rigidbody.isKinematic = true;
                _movement.EnableMoveComponent();
                _groundDetector.DisableGroundChecker();
                
                _isAttacked = false;
                IsAttacked = false;
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            progress.SkinName = _skinService.GetSkinData.SkinName;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.Gold += GoldService.CurrentCount;
            progress.SkinName = _skinService.GetSkinData.SkinName;
        }

        public override void YouWin()
        {
            _decal.HideDecal();
            GetComponent<NavMeshAgent>().enabled = false;

            int randomIndex = Random.Range(0, 3);
            _girlService.StartGirlDancing(randomIndex);
            _stickmanAnimator.PlayWinner(randomIndex);
            
            base.YouWin();
        }

        private IEnumerator ImmortalState()
        {
            _immortalAnimation.RunAnim(_immortalTime);
            
            float timer = _immortalTime;

            while (timer >= 0.0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            _immortalTimeCoroutine = null;
            _immortalAnimation.StopAnim();
        }

        private void OnDestroy()
        {
            _groundDetector.OnGrounded -= OnGrounded;
        }
    }
}
