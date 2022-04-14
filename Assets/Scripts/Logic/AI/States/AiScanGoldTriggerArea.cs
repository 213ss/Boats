using System.Collections;
using Actors;
using Enemy;
using Infrastructure.Data.ScriptableObjects;
using Infrastructure.Services.LevelService;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Logic.AI.States
{
    public class AiScanGoldTriggerArea : IState
    {
        private readonly AiData _aiData;
        private readonly Actor _ownerActor;
        private readonly IAIMovement _movementService;
        
        private readonly AiStateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;

        private Coroutine _scanGoldTriggerCoroutine;

        public AiScanGoldTriggerArea(AiStateMachine stateMachine, DiContainer diContainer, Actor ownerActor, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _ownerActor = ownerActor;
            _coroutineRunner = coroutineRunner;

            ILevelService levelService = diContainer.Resolve<ILevelService>();

            _aiData = levelService.AIParametersData;
            
            _movementService = _ownerActor.GetComponent<IAIMovement>();
        }

        public void Enter()
        {
            if (CheckIslandLast())
            {
                SendActorToFinalPoint();
                _stateMachine.Enter<AiWaitState>();
                return;
            }

            _scanGoldTriggerCoroutine = _coroutineRunner.StartCoroutine(ScanGoldTriggerArea());
        }

        public void Exit()
        {
            if(_scanGoldTriggerCoroutine != null)
                _coroutineRunner.StopCoroutine(_scanGoldTriggerCoroutine);
        }

        private IEnumerator ScanGoldTriggerArea()
        {
            float timerScan = _aiData.GetTimeScanGoldTrigger() + Random.Range(0.0f, 5.0f);

            while (timerScan >= 0.0f)
            {
                if (CheckGoldToTravel())
                {
                    _stateMachine.Enter<AiGoToNextIslandState>();
                    yield break;
                }
                
                if (_movementService.IsDisableMovement == false)
                {
                    if (_movementService.IsMovement == false)
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

                timerScan -= Time.deltaTime;
                yield return null;
            }
            
            _stateMachine.Enter<AiTriggerGoldSearchState>();
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