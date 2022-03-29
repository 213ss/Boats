using System;
using System.Collections;
using Actors;
using Enemy;
using Logic.Weapon.Weapons;
using Scripts.Infrastructure;
using UnityEngine;

namespace Logic.AI.States
{
    public class AiTriggerGoldSearchState : IState
    {
        private readonly Actor _ownerActor;
        private readonly IShovelWeapon _shovelWeapon;
        private readonly IAIMovement _movementService;
        
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AiStateMachine _stateMachine;
        
        private Coroutine _searchGoldTriggerCoroutine;

        public AiTriggerGoldSearchState(AiStateMachine stateMachine, Actor ownerActor, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _ownerActor = ownerActor;
            _coroutineRunner = coroutineRunner;

            _movementService = _ownerActor.GetComponent<IAIMovement>();
            _shovelWeapon = _ownerActor.GetComponent<IShovelWeapon>();
        }


        public void Enter()
        {
            if (_ownerActor.CurrentIsland.IsLast)
            {
                _movementService.SetDestination(_ownerActor.CurrentIsland.GetFinalPoint.position);
                _stateMachine.Enter<AiWaitState>();
                return;
            }

            _searchGoldTriggerCoroutine = _coroutineRunner.StartCoroutine(SearchGoldTrigger());
        }

        public void Exit()
        {
            if(_searchGoldTriggerCoroutine != null)
                _coroutineRunner.StopCoroutine(_searchGoldTriggerCoroutine);
        }

        private IEnumerator SearchGoldTrigger()
        {
            while (_ownerActor.CurrentIsland.GetCountActiveTriggers() > 0)
            {
                if (CheckGoldToTravel())
                {
                    _stateMachine.Enter<AiTravelState>();
                    yield break;
                }
                
                if (_movementService.IsDisableMovement == false)
                {
                    if (_movementService.IsMovement)
                    {
                        if (_shovelWeapon.IsStayInGoldArea)
                        {
                            _stateMachine.Enter<AiDigGoldState>();
                            yield break;
                        }
                    }
                    else
                    {
                        if (_shovelWeapon.IsStayInGoldArea)
                        {
                            _stateMachine.Enter<AiDigGoldState>();
                            yield break;
                        }
                        
                        if (TrySendActorToRandomGoldTrigger() == false)
                        {
                            _stateMachine.Enter<AiCoinSearch>();
                            yield break;
                        }
                    }
                }

                yield return null;
            }
            
            _stateMachine.Enter<AiCoinSearch>();
        }

        private bool TrySendActorToRandomGoldTrigger()
        {
            if (_ownerActor.CurrentIsland == null)
                throw new NullReferenceException("TryGetIslandForActor: " + _ownerActor.ActorName);

            Vector3 randomTriggerPosition = _ownerActor.CurrentIsland.GetPointRandomGoldTrigger();
            if (randomTriggerPosition.y <= -9.0f)
            {
                return false;
            }

            randomTriggerPosition.y = _ownerActor.ActorTransform.position.y;
            _movementService.SetDestination(randomTriggerPosition);
            return true;
        }

        private bool CheckGoldToTravel()
        {
            return _ownerActor.GoldService.CurrentCount >= _ownerActor.CurrentIsland.CostDelivery;
        }
    }
}