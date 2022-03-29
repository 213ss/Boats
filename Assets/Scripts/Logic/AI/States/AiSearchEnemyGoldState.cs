using System.Collections;
using Actors;
using Enemy;
using Scripts.Infrastructure;
using UnityEngine;

namespace Logic.AI.States
{
    public class AiSearchEnemyGoldState : IState
    {
        private readonly Actor _ownerActor;
        private readonly IAIMovement _movement;

        private readonly AiStateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _scanEnemyCoroutine;

        public AiSearchEnemyGoldState(AiStateMachine stateMachine, Actor ownerActor, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _ownerActor = ownerActor;
            _coroutineRunner = coroutineRunner;

            _movement = ownerActor.GetComponent<IAIMovement>();
        }

        public void Enter()
        {
            _scanEnemyCoroutine = _coroutineRunner.StartCoroutine(SearchEnemy());
        }

        public void Exit()
        {
            if(_scanEnemyCoroutine != null)
                _coroutineRunner.StopCoroutine(_scanEnemyCoroutine);
        }

        private IEnumerator SearchEnemy()
        {
            Actor target = GetRandomTarget();
            
            while (true)
            {
                if (CheckNeededGoldToTravel())
                {
                    _stateMachine.Enter<AiGoToNextIslandState>();
                    yield break;
                }
                
                if (target != null && target.IsTravel == false && target.IsDroppedOutGame == false)
                {
                    if (_movement.IsDisableMovement == false)
                    {
                        _movement.SetDestination(target.ActorTransform.position);

                        if (Vector3.Distance(_ownerActor.ActorTransform.position, target.ActorTransform.position) < _ownerActor.ApproachDistance)
                        {
                            target = GetRandomTarget();
                            if(target == null)
                                _stateMachine.Enter<AiCoinSearch>();
                        }
                    }
                }
                else
                {
                    target = GetRandomTarget();
                    if (target == null)
                    {
                        _stateMachine.Enter<AiCoinSearch>();
                    }
                }

                yield return null;
            }
        }

        private Actor GetRandomTarget()
        {
            Actor[] targets = _ownerActor.CurrentIsland.GetActorsInIslandExistGold();

            if (targets == null)
                return null;

            if (targets.Length == 1)
            {
                return targets[0];   
            }

            if (targets.Length > 1)
                return targets[Random.Range(0, targets.Length - 1)];

            return null;
        }

        private bool CheckNeededGoldToTravel()
        {
            return _ownerActor.GoldService.CurrentCount >= _ownerActor.CurrentIsland.CostDelivery;
        }
    }
}