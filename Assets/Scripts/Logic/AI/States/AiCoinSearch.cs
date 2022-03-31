using System.Collections;
using Actors;
using Enemy;
using Infrastructure.Services.Islands;
using Logic.GoldLoot;
using Logic.Island;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Logic.AI.States
{
    public class AiCoinSearch : IState
    {
        private readonly AiStateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Actor _ownerActor;
        private readonly IIslandService _islandService;

        private Actor _currentTarget;
        
        private Coroutine _searchCoinsCoroutine;
        private BaseIsland _island;
        private readonly IAIMovement _movement;
        private Collider[] _bufferGoldLoot = new Collider[5];
        private GoldLoots _currentLootTarget;

        public AiCoinSearch(AiStateMachine stateMachine, DiContainer diContainer, ICoroutineRunner coroutineRunner, Actor ownerActor)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _ownerActor = ownerActor;

            _movement = _ownerActor.GetComponent<IAIMovement>();
            
            _islandService = diContainer.Resolve<IIslandService>();
        }

        public void Enter() 
        {
            _island = _islandService.TryGetIslandForActor(_ownerActor);
            _searchCoinsCoroutine = _coroutineRunner.StartCoroutine(SearchCoin());
        }

        public void Exit()
        {
            if(_searchCoinsCoroutine != null)
                _coroutineRunner.StopCoroutine(_searchCoinsCoroutine);
        }

        private IEnumerator SearchCoin()
        {
            GoldLoots targetLoot = GetRandomLoot();
            
            while (true)
            {
                if (CheckNeededGoldToTravel())
                {
                    _stateMachine.Enter<AiGoToNextIslandState>();
                    yield break;
                }

                if (_island.GetCountGoldLoots() > 0 && targetLoot != null)
                {
                    if (_movement.IsDisableMovement == false)
                    {
                        _movement.SetDestination(targetLoot.transform.position);
                        if (Vector3.Distance(_ownerActor.ActorTransform.position, targetLoot.transform.position) < _ownerActor.ApproachDistance)
                        {
                            targetLoot = GetRandomLoot();
                            if (targetLoot == null)
                            {
                                if (_island.GetCountActorExistGold() > 0)
                                {
                                    _stateMachine.Enter<AiSearchEnemyGoldState>();
                                }
                                else
                                {
                                    _stateMachine.Enter<AiScanGoldTriggerArea>();
                                }
                            }
                        }
                    }else
                    {
                        if(_ownerActor.IsAttacked == false)
                            _movement.EnableMoveComponent();
                    }
                }
                else
                {
                    GoldLoots loot = GetRandomLoot();
                    if (loot != null)
                    {
                        targetLoot = loot;
                    }
                    else
                    {
                        if (_island.GetCountActorExistGold() > 0)
                        {
                            _stateMachine.Enter<AiSearchEnemyGoldState>();
                        }
                        else
                        {
                            _stateMachine.Enter<AiScanGoldTriggerArea>();
                        }
                    }
                }
                
                yield return null;
            }
        }

        private GoldLoots GetRandomLoot()
        {
            GoldLoots[] loots = _island.GetGoldLoots();

            if (loots == null)
                return null;

            if (loots.Length == 1)
            {
                return loots[0];
            }

            if (loots.Length > 0)
                return loots[Random.Range(0, loots.Length - 1)];

            return null;
        }
        
        private bool CheckNeededGoldToTravel()
        {
            return _ownerActor.GoldService.CurrentCount >= _ownerActor.CurrentIsland.CostDelivery;
        }
        
        
    }
}