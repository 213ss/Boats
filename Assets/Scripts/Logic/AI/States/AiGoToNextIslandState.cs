using System.Collections;
using Actors;
using Enemy;
using Infrastructure.Services.Islands;
using Logic.Island;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Logic.AI.States
{
    public class AiGoToNextIslandState : IState
    {
        private readonly AiStateMachine _stateMachine;
        private readonly Actor _aiOwner;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IIslandService _islandService;

        private IAIMovement _movement;

        private Coroutine _waitGoToPearseCoroutine;
        private BaseIsland _island;

        public AiGoToNextIslandState(AiStateMachine stateMachine, DiContainer diContainer, Actor aiOwner, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _aiOwner = aiOwner;
            _coroutineRunner = coroutineRunner;

            _islandService = diContainer.Resolve<IIslandService>();

            _movement = aiOwner.GetComponent<IAIMovement>();
        }
        
        public void Enter()
        {
            _movement.SetDestination(_aiOwner.CurrentIsland.PierPosition);

            _island = _islandService.TryGetIslandForActor(_aiOwner);
            _waitGoToPearseCoroutine = _coroutineRunner.StartCoroutine(WaitGoToPearse());
        }

        public void Exit()
        {
            _coroutineRunner.StopCoroutine(_waitGoToPearseCoroutine);
        }

        private IEnumerator WaitGoToPearse()
        {
            while (_aiOwner.IsTravel == false)
            {
                if (_aiOwner.GoldService.CurrentCount < _island.CostDelivery)
                {
                    if (_island.GetCountActiveTriggers() > 0)
                    {
                        _stateMachine.Enter<AiTriggerGoldSearchState>();
                        yield break;
                    }
                    else
                    {
                        _stateMachine.Enter<AiCoinSearch>();
                        yield break;
                    }
                }

                if (_movement.IsAFK())
                {
                    _movement.SetDestination(_aiOwner.CurrentIsland.PierPosition);
                }
                
                yield return null;
            }
            
            _stateMachine.Enter<AiTravelState>();
        }
    }
}