using System.Collections;
using Actors;
using Actors.Actors;
using Enemy;
using Scripts.Infrastructure;
using UnityEngine;

namespace Logic.AI.States
{
    public class AiWaitPlayerActorState : IState
    {
        private readonly AiStateMachine _stateMachine;
        private readonly Actor _ownerActor;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly EnemyActor _enemyActor;
        private readonly IAIMovement _movementService;
        
        private Coroutine _waitPlayerCoroutine;

        public AiWaitPlayerActorState(AiStateMachine stateMachine, Actor ownerActor, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _ownerActor = ownerActor;
            _coroutineRunner = coroutineRunner;

            _movementService = _ownerActor.GetComponent<IAIMovement>();
            _enemyActor = _ownerActor as EnemyActor;
        }

        public void Enter()
        {
            if (CheckIslandLast())
            {
                SendActorToFinalPoint();
                _stateMachine.Enter<AiWaitState>();
                return;
            }
            
            _waitPlayerCoroutine = _coroutineRunner.StartCoroutine(WaitPlayerActor());
        }

        public void Exit()
        {
            if(_waitPlayerCoroutine != null)
                _coroutineRunner.StopCoroutine(_waitPlayerCoroutine);
        }

        private IEnumerator WaitPlayerActor()
        {
            while (_enemyActor.CurrentIsland.ActorExistOnIsland(_enemyActor.PlayerActor) == false)
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
                        
                    }
                    else
                    {
                        SendActorToRandomPoint();
                    }
                }else
                {
                    if (_ownerActor.IsAttacked == false)
                        _movementService.EnableMoveComponent();
                }
                
                if (_movementService.IsAFK())
                {
                    SendActorToRandomPoint();
                }
                
                yield return null;
            }

            SendActorToRandomPoint();
            yield return new WaitForSeconds(3.0f);
            _stateMachine.Enter<AiScanGoldTriggerArea>();
        }
        
        private bool CheckGoldToTravel()
        {
            return _ownerActor.GoldService.CurrentCount >= _ownerActor.CurrentIsland.CostDelivery;
        }
        
        private bool CheckIslandLast()
        {
            return _ownerActor.CurrentIsland.IsLast;
        }
        
        private void SendActorToFinalPoint()
        {
            _movementService.SetDestination(_ownerActor.CurrentIsland.GetFinalPoint.position);
        }
        
        private void SendActorToRandomPoint()
        {
            _movementService.SetDestination(_ownerActor.CurrentIsland.GetRandomPointInIsland());
        }
    }
}