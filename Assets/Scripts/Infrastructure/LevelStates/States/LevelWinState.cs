using System.Collections;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Screen;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Infrastructure.LevelStates.States
{
    public class LevelWinState: ILevelState
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IScreenService _screenService;
        private readonly ISaveLoadService _saveLoadService;

        public LevelWinState(DiContainer diContainer, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _screenService = diContainer.Resolve<IScreenService>();
            _saveLoadService = diContainer.Resolve<ISaveLoadService>();
        }

        public void Enter()
        {
            _saveLoadService.SaveProgress();
            _coroutineRunner.StartCoroutine(DelayShowScreen());
        }

        public void Exit()
        {
            
        }
        
        private IEnumerator DelayShowScreen()
        {
            float timer = 5.0f;
            
            while (timer >= 0.0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            
            _screenService.HideAllScreens();
            _screenService.ShowWinScreen();            
        }
    }
}