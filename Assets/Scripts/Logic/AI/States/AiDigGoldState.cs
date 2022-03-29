using System.Collections;
using Actors;
using Logic.Weapon.Weapons;
using Scripts.Infrastructure;
using UnityEngine;

namespace Logic.AI.States
{
    public class AiDigGoldState : IState
    {
        private readonly ShovelWeapon _shovelWeapon;
        private readonly AiStateMachine _stateMachine;
        private readonly Actor _aiOwner;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IMovement _movement;

        private Coroutine _checkCoroutine;


        public AiDigGoldState(AiStateMachine stateMachine, Actor aiOwner, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _aiOwner = aiOwner;
            _coroutineRunner = coroutineRunner;
            _shovelWeapon = _aiOwner.GetComponent<ShovelWeapon>();

            _movement = _aiOwner.GetComponentInParent<IMovement>();
        }
        
        public void Enter()
        {
            _movement.DisableMoveComponent();
            
            _shovelWeapon.OnExcavated += OnExcavated;
            _shovelWeapon.Excavate();

            _checkCoroutine = _coroutineRunner.StartCoroutine(ExcavatedCheck());
        }

        public void Exit()
        {
            _shovelWeapon.OnExcavated -= OnExcavated;
            
            _movement.EnableMoveComponent();
            
            if(_checkCoroutine != null)
                _coroutineRunner.StopCoroutine(_checkCoroutine);
        }

        private void OnExcavated()
        {
            if (_aiOwner.GoldService.CurrentCount >= _aiOwner.CurrentIsland.CostDelivery)
            {
                _stateMachine.Enter<AiGoToNextIslandState>();
                return;
            }
            
            _stateMachine.Enter<AiScanGoldTriggerArea>();
        }

        private IEnumerator ExcavatedCheck()
        {
            var timer = 5.0f;
            while (timer >= 0.0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            _stateMachine.Enter<AiScanGoldTriggerArea>();
        }
    }
}