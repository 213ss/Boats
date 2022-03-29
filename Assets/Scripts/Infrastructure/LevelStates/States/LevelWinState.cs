using Actors;
using Infrastructure.Services.AnalyticsServices;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Screen;
using UnityEngine;
using Zenject;

namespace Infrastructure.LevelStates.States
{
    public class LevelWinState: ILevelState
    {
        private readonly IScreenService _screenService;
        private readonly ISaveLoadService _saveLoadService;

        public LevelWinState(DiContainer diContainer)
        {
            _screenService = diContainer.Resolve<IScreenService>();
            _saveLoadService = diContainer.Resolve<ISaveLoadService>();
        }

        public void Enter()
        {
            _saveLoadService.SaveProgress();
            
            GameObject actorObject = GameObject.FindGameObjectWithTag("Player");
            Actor actor = actorObject.GetComponent<Actor>();
            
            GameAnalyticsService.Instance.PlayerProgress(LevelProgressingStatus.Complete, actor.GoldService.CurrentCount);
            
            _screenService.HideAllScreens();
            _screenService.ShowWinScreen();
        }

        public void Exit()
        {
            
        }
    }
}