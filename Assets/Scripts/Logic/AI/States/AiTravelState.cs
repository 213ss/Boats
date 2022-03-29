using System.Collections;
using Actors;
using Scripts.Infrastructure;
using UnityEngine;

namespace Logic.AI.States
{
    public class AiTravelState : IState
    {
        private readonly AiStateMachine _stateMachine;
        private readonly Actor _aiOwner;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IMovement _movement;

        private Coroutine _waitTravelCoroutine;

        public AiTravelState(AiStateMachine stateMachine, Actor aiOwner, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _aiOwner = aiOwner;
            _coroutineRunner = coroutineRunner;

            _movement = _aiOwner.GetComponent<IMovement>();
        }

        public void Enter()
        {
            _movement.DisableMoveComponent();
            _waitTravelCoroutine = _coroutineRunner.StartCoroutine(WaitTravel());
        }

        public void Exit()
        {
            _coroutineRunner.StopCoroutine(_waitTravelCoroutine);
        }

        private IEnumerator WaitTravel()
        {
            while (_aiOwner.IsTravel)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            _stateMachine.Enter<AiWaitPlayerActorState>();
        }
    }
}