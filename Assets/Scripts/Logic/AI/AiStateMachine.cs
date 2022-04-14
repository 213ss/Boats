using System;
using System.Collections.Generic;
using Actors;
using Logic.AI.States;
using Scripts.Infrastructure;
using Zenject;

namespace Logic.AI
{
    public class AiStateMachine
    {
        private Dictionary<Type, IState> _states;
        private IState _currentState;
        
        public AiStateMachine(Actor aiOwner, DiContainer diContainer, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(AiWaitPlayerActorState)] = new AiWaitPlayerActorState(this, aiOwner, coroutineRunner),
                [typeof(AiScanGoldTriggerArea)] = new AiScanGoldTriggerArea(this, diContainer, aiOwner, coroutineRunner),
                [typeof(AiTriggerGoldSearchState)] = new AiTriggerGoldSearchState(stateMachine: this, aiOwner, coroutineRunner),
                [typeof(AiDigGoldState)] = new AiDigGoldState(stateMachine: this, aiOwner, coroutineRunner),
                [typeof(AiGoToNextIslandState)] = new AiGoToNextIslandState(stateMachine: this, diContainer, aiOwner, coroutineRunner),
                [typeof(AiDroppedOutGameState)] = new AiDroppedOutGameState(),
                [typeof(AiTravelState)] = new AiTravelState(stateMachine: this, aiOwner, coroutineRunner),
                [typeof(AiCoinSearch)] = new AiCoinSearch(stateMachine: this, diContainer, coroutineRunner, aiOwner),
                [typeof(AiPatrolState)] = new AiPatrolState(this, diContainer, aiOwner, coroutineRunner),
                [typeof(AiWinnerState)] = new AiWinnerState(),
                [typeof(AiWaitState)] = new AiWaitState()
            };
        }

        public void Enter<TState>() where TState : IState
        {
            if (_currentState != null)
                _currentState?.Exit();

            IState state = _states[typeof(TState)];
            _currentState = state;
            state.Enter();
        }
    }
}