using Actors;
using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Infrastructure.Services.Game;
using Infrastructure.Services.Islands;
using Infrastructure.Services.LevelService;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.SkinChanger;
using Logic.UI.GoldWidget;
using Scripts.Infrastructure.Services.Camera;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;
using Zenject;

namespace Infrastructure.LevelStates.States
{
    public class LoadLevelState : ILevelState
    {
        private readonly StateMachine _stateMachine;
        private readonly ICameraFollow _cameraFollow;
        private readonly IGameFactory _gameFactory;
        private readonly ISkinChanger _skinChanger;
        private readonly IIslandService _islandService;
        
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistenceProgressServices _persistenceProgress;
        private readonly ILevelService _levelService;

        public LoadLevelState(StateMachine stateMachine, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _cameraFollow = diContainer.Resolve<ICameraFollow>();
            _gameFactory = diContainer.Resolve<IGameFactory>();
            _skinChanger = diContainer.Resolve<ISkinChanger>();
            _islandService = diContainer.Resolve<IIslandService>();
            _saveLoadService = diContainer.Resolve<ISaveLoadService>();
            _persistenceProgress = diContainer.Resolve<IPersistenceProgressServices>();
            _levelService = diContainer.Resolve<ILevelService>();
        }

        public void Enter()
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            GameObject mainActor = _gameFactory.CreateGameObject(AssetsPath.MainActorPath, spawnPoint.transform.position);
            
            GameService gameService = Object.FindObjectOfType<GameService>();
            
            _levelService.SetPlayerActor(mainActor.GetComponent<Actor>());
            _levelService.SetGameService(gameService);
            
            Object.FindObjectOfType<PlayerGoldWidget>().SetGoldService(mainActor.GetComponent<IGold>());
            
            _cameraFollow.SetFollow(mainActor.transform);
            
            _skinChanger.InitSkinContainer();
            _skinChanger.SetDefaultMainActorSkin(mainActor.GetComponent<ISkin>());
            
            _islandService.SetActorToStartIsland(mainActor.GetComponent<Actor>());
            
            LoadPlayerProgress();

            _stateMachine.Enter<PressToTapState>();
        }

        private void LoadPlayerProgress()
        {
            _persistenceProgress.progress = _saveLoadService.LoadProgress();
        }

        public void Exit()
        {
            
        }
    }
}