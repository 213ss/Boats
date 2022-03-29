using System.Collections;
using Infrastructure.Services.AnalyticsServices;
using Infrastructure.Services.InputServices;
using Infrastructure.Services.Screen;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Infrastructure.LevelStates.States
{
    public class PressToTapState : ILevelState
    {
        private readonly StateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IScreenService _screenService;
        private readonly IInputServices _playerInputService;
        
        private Coroutine _waitPressTapCoroutine;


        public PressToTapState(StateMachine stateMachine, DiContainer diContainer, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;

            _screenService = diContainer.Resolve<IScreenService>();
            _playerInputService = diContainer.Resolve<IInputServices>();
        }

        public void Enter()
        {
            _screenService.ShowPressTapScreen();

            _waitPressTapCoroutine = _coroutineRunner.StartCoroutine(WaitPressTap());
        }

        public void Exit()
        {
            _screenService.HideAllScreens();
            _coroutineRunner.StopCoroutine(_waitPressTapCoroutine);
        }

        private IEnumerator WaitPressTap()
        {
            while (_playerInputService.IsLeftMouseButtonUp() == false)
            {
                yield return null;
            }
            
            
            
            _stateMachine.Enter<LevelProcessState>();
        }
    }
}