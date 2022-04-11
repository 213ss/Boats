using System.Collections;
using Actors;
using Infrastructure.Services.Screen;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Infrastructure.LevelStates.States
{
    public class LevelLoseState: ILevelState
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IScreenService _screenService;

        public LevelLoseState(DiContainer diContainer, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _screenService = diContainer.Resolve<IScreenService>();
        }

        public void Enter()
        {
            var actor = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            actor.DisableActor();

            _coroutineRunner.StartCoroutine(DelayShowScreen());
        }
        

        public void Exit()
        {
            
        }

        private IEnumerator DelayShowScreen()
        {
            float timer = 2.0f;
            
            while (timer >= 0.0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            
            _screenService.HideAllScreens();
            _screenService.ShowLoseScreen();            
        }
    }
}