using Actors;
using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Infrastructure.Services.Islands;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.SkinChanger;
using Logic.UI.GoldWidget;
using Scripts.Infrastructure.Services.Camera;
using Scripts.Infrastructure.Services.Gold;
using Scripts.Logic;
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
        private readonly IPersistenceProgressServices _persistanceProgress;

        public LoadLevelState(StateMachine stateMachine, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _cameraFollow = diContainer.Resolve<ICameraFollow>();
            _gameFactory = diContainer.Resolve<IGameFactory>();
            _skinChanger = diContainer.Resolve<ISkinChanger>();
            _islandService = diContainer.Resolve<IIslandService>();
            _saveLoadService = diContainer.Resolve<ISaveLoadService>();
            _persistanceProgress = diContainer.Resolve<IPersistenceProgressServices>();
        }

        public void Enter()
        {
            var spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            var mainActor = _gameFactory.CreateGameObject(AssetsPath.MainActorPath, spawnPoint.transform.position);
            
            GameObject.FindObjectOfType<PlayerGoldWidget>().SetGoldService(mainActor.GetComponent<IGold>());
            
            _cameraFollow.SetFollow(mainActor.transform);
            
            _skinChanger.InitSkinContainer();
            
            _skinChanger.SetDefaultMainActorSkin(mainActor.GetComponent<ISkin>());
            
            _islandService.SetActorToStartIsland(mainActor.GetComponent<Actor>());
            
            LoadPlayerProgress();

            _stateMachine.Enter<PressToTapState>();
        }

        private void LoadPlayerProgress()
        {
            _persistanceProgress.progress = _saveLoadService.LoadProgress();
        }

        public void Exit()
        {
            
        }
    }
}