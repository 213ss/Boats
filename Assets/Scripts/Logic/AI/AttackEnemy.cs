using System.Collections;
using Actors;
using Infrastructure.Data.ScriptableObjects;
using Logic.Triggers;
using Logic.Weapon.Weapons;
using Scripts.Infrastructure.Data;
using UnityEngine;

namespace Logic.AI
{
    public class AttackEnemy : MonoBehaviour
    {
        [SerializeField] private AiData _aiData;

        [Header("Component references")]
        [SerializeField] private Actor _ownerActor;
        [SerializeField] private ShovelWeapon _weapon;
        [SerializeField] private ActorDetector _actorDetector;

        private float _timeReloadsAttack;

        private bool _isAttacking;
        private Coroutine _reloadAttackCoroutine;

        private void Start()
        {
            _ownerActor = GetComponent<Actor>();
            _actorDetector.OnDetectActor += OnActorDetect;

            _timeReloadsAttack = _aiData.GetTimeReloadsAttack();
        }

        private void OnActorDetect(Actor actor)
        {
            if(_ownerActor.IsDroppedOutGame) return;
            if(_ownerActor.IsTravel) return;
            if(_isAttacking) return;
            if(_ownerActor.IsWin) return;
            
            if (actor == null)
            {
                return;
            }

            if (_ownerActor.ActorTeam != actor.ActorTeam)
            {
                if (_ownerActor.ActorTeam == Team.Player_0)
                {
                    if (_weapon.IsExcavated == false && actor.GoldService.CurrentCount >= 1)
                    {
                        _weapon.Attack();
                        _isAttacking = true;
                        if(_reloadAttackCoroutine == null)
                            _reloadAttackCoroutine = StartCoroutine(ReloadAttackTimer());
                        
                        return;
                    }
                }
                
                if (_isAttacking == false && _weapon.IsExcavated == false && _aiData.IsChanceAttack())
                {
                    _weapon.Attack();
                    _isAttacking = true;
                    if (_reloadAttackCoroutine == null)
                        _reloadAttackCoroutine = StartCoroutine(ReloadAttackTimer());
                }
                else
                {
                    if (_reloadAttackCoroutine == null)
                        _reloadAttackCoroutine = StartCoroutine(ReloadAttackTimer());
                }
            }
        }

        private IEnumerator ReloadAttackTimer()
        {
            float timer = _timeReloadsAttack;
            while (timer >= 0.0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            _isAttacking = false;
            _reloadAttackCoroutine = null;
        }

        private void OnDestroy()
        {
            _actorDetector.OnDetectActor -= OnActorDetect;
        }
    }
}