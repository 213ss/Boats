using Actors;
using Logic.AI.States;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Logic.AI
{
    public class AiBrain : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Actor _aiOwner;
        
        private AiStateMachine _stateMachine;
        private DiContainer _diContainer;

        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Start()
        {
            _stateMachine = new AiStateMachine(_aiOwner, _diContainer, this);
            DisableAI();
        }

        private void Update()
        {
            if (_aiOwner.IsDroppedOutGame)
            {
                _stateMachine.Enter<AiDroppedOutGameState>();
            }
        }

        public void EnableAI()
        {
            _stateMachine.Enter<AiWaitPlayerActorState>();
        }

        public void DisableAI()
        {
            _stateMachine.Enter<AiWaitState>();
        }
    }
}