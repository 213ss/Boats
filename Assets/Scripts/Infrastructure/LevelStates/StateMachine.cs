using System;
using System.Collections.Generic;
using Infrastructure.LevelStates.States;
using Scripts.Infrastructure;
using Zenject;

namespace Infrastructure.LevelStates
{
    public class StateMachine
    {
        private Dictionary<Type, ILevelState> _states;
        private ILevelState _currentState;
        
        public StateMachine(DiContainer diContainer, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, ILevelState>
            {
                [typeof(LoadLevelState)] = new LoadLevelState(this, diContainer),
                [typeof(PressToTapState)] = new PressToTapState(this, diContainer, coroutineRunner),
                [typeof(LevelProcessState)] = new LevelProcessState(this, coroutineRunner),
                [typeof(LevelWinState)] = new LevelWinState(diContainer),
                [typeof(LevelLoseState)] = new LevelLoseState(this, diContainer)
                
            };
        }

        public void Enter<TLevelState>() where TLevelState : ILevelState
        {
            if (_currentState != null)
                _currentState?.Exit();

            ILevelState state = _states[typeof(TLevelState)];
            _currentState = state;
            state.Enter();
        }

    }
}