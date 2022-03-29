using System.Collections;
using Actors;
using Enemy;
using Infrastructure.AssetManagment;
using Infrastructure.Data.ScriptableObjects;
using Logic.Weapon.Weapons;
using Scripts.Infrastructure;
using UnityEngine;

namespace Logic.AI.States
{
    public class AiScanGoldTriggerArea : IState
    {
        private readonly AiData _aiData;
        private readonly Actor _ownerActor;
        private readonly IAIMovement _movementService;
        
        private readonly AiStateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IShovelWeapon _shovelWeapon;
        
        private Coroutine _scanGoldTriggerCoroutine;

        public AiScanGoldTriggerArea(AiStateMachine stateMachine, Actor ownerActor, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _ownerActor = ownerActor;
            _coroutineRunner = coroutineRunner;

            _aiData = Resources.Load<AiData>(AssetsPath.AiTriggerSearchData);
            
            _movementService = _ownerActor.GetComponent<IAIMovement>();
            _shovelWeapon = _ownerActor.GetComponent<IShovelWeapon>();
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
                    _stateMachine.Enter<AiTravelState>();
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