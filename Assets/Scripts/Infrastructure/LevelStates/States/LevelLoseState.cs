using Actors;
using Infrastructure.Services.AnalyticsServices;
using Infrastructure.Services.Screen;
using UnityEngine;
using Zenject;

namespace Infrastructure.LevelStates.States
{
    public class LevelLoseState: ILevelState
    {
        private readonly StateMachine _stateMachine;
        private readonly IScreenService _screenService;

        public LevelLoseState(StateMachine stateMachine, DiContainer diContainer)
        {
            _stateMachine = stateMachine;

            _screenService = diContainer.Resolve<IScreenService>();
        }

        public void Enter()
        {
            var actor = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            actor.DisableActor();
            
            LevelLoseSendAnalyticsEvent();
            _screenService.HideAllScreens();
            _screenService.ShowLoseScreen();
        }

        private static void LevelLoseSendAnalyticsEvent()
        {
            GameAnalyticsService.Instance.PlayerProgress(LevelProgressingStatus.Fail, 0.0f);
        }

        public void Exit()
        {
            
        }
    }
}