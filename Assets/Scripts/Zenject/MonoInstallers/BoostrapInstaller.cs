using Actors;
using Enemy;
using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Infrastructure.Services.Gold;
using Infrastructure.Services.GoldLoot;
using Infrastructure.Services.GroundDetector;
using Infrastructure.Services.InputServices;
using Infrastructure.Services.Islands;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Screen;
using Infrastructure.Services.SkinChanger;
using Infrastructure.Services.UIDirect;
using Infrastructure.Services.Vibrate;
using Logic;
using Logic.Triggers;
using Scripts.Infrastructure.Services.Camera;
using Scripts.Infrastructure.Services.Gold;

namespace Zenject.MonoInstallers
{
    public class BoostrapInstaller: MonoInstaller
    {
        public InputServices _inputServices;
        public UIFollow _uiFollow;
        public AssetProvider _assetProvider;
        public GameFactory _gameFactory;
        public CameraFollow _cameraFollow;
        public SkinChanger _skinChanger;
        public GoldIndicator goldIndicator;
        public VfxFromTo _vfxFromTo;
        public IslandService _islandService;
        public ScreenService _screenService;
        public Vibrate _Vibrate;
        public GoldLootService _goldLootService;
        public UIDirectToWorldObject _uiIndicatorService;


        public override void InstallBindings()
        {
            InputServiceInstaller();
            
            AssetProviderInstalls();
            GameFactoryInstalls();
            
            UIFollowServicesInstaller();
            CameraFollowInstaller();
            UIDirectionService();

            SkinChangeInstaller();

            ActorDetectorInstaller();
            GoldInstaller();
            GoldIndicatorInstaller();

            AIMovementInstaller();
            MovementIstanller();
            GroundDetector();
            
            VfxFromToInstaller();
            IslandServiceInstaller();
            ScreenServiceInstaller();

            PersistenceProgressServices();
            SaveLoadService();
            VibrateService();
            GoldLoot();
            SkinService();
        }

        private void GoldLoot()
        {
            Container
                .Bind<IGoldLootService>()
                .To<GoldLootService>()
                .FromInstance(_goldLootService)
                .AsSingle()
                .NonLazy();
        }
        
        private void SkinService()
        {
            Container
                .Bind<ISkin>()
                .To<Skin>()
                .FromComponentInParents();
        }

        private void VibrateService()
        {
            Container
                .Bind<IVibrate>()
                .To<Vibrate>()
                .FromInstance(_Vibrate)
                .AsSingle()
                .NonLazy();
        }

        private void PersistenceProgressServices()
        {
            Container
                .Bind<IPersistenceProgressServices>()
                .To<PersistenceProgress>()
                .FromNew()
                .AsSingle();
        }

        private void SaveLoadService()
        {
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .FromNew()
                .AsSingle();
        }

        private void ScreenServiceInstaller()
        {
            Container
                .Bind<IScreenService>()
                .To<ScreenService>()
                .FromInstance(_screenService)
                .AsSingle()
                .NonLazy();
        }
        
        private void IslandServiceInstaller()
        {
            Container
                .Bind<IIslandService>()
                .To<IslandService>()
                .FromInstance(_islandService)
                .AsSingle()
                .NonLazy();
        }

        private void VfxFromToInstaller()
        {
            Container
                .Bind<IVfxFromTo>()
                .To<VfxFromTo>()
                .FromInstance(_vfxFromTo)
                .AsSingle()
                .NonLazy();
        }

        private void GoldInstaller()
        {
            Container
                .Bind<IGoldChanger>()
                .To<Gold>()
                .FromComponentInParents();

            Container
                .Bind<IGold>()
                .To<Gold>()
                .FromComponentInParents();

        }

        private void GoldIndicatorInstaller()
        {
            Container
                .Bind<IGoldIndicator>()
                .To<GoldIndicator>()
                .FromInstance(goldIndicator).NonLazy();
        }

        private void SkinChangeInstaller()
        {
            Container
                .Bind<ISkinChanger>()
                .To<SkinChanger>()
                .FromInstance(_skinChanger)
                .AsSingle().NonLazy();
        }

        private void CameraFollowInstaller()
        {
            Container
                .Bind<ICameraFollow>()
                .To<CameraFollow>()
                .FromInstance(_cameraFollow)
                .AsSingle().NonLazy();
        }
        

        private void UIFollowServicesInstaller()
        {
            Container
                .Bind<IUIFollow>()
                .To<UIFollow>()
                .FromInstance(_uiFollow)
                .AsSingle().NonLazy();
        }
        private void UIDirectionService()
        {
            Container
                .Bind<IUIIndicatorService>()
                .To<UIDirectToWorldObject>()
                .FromInstance(_uiIndicatorService)
                .AsSingle().NonLazy();
        }

        private void ActorDetectorInstaller()
        {
            Container
                .Bind<IActorDetector>()
                .To<ActorDetector>()
                .FromComponentInParents();
        }

        private void AssetProviderInstalls()
        {
            Container
                .Bind<IAssets>()
                .To<AssetProvider>()
                .FromInstance(_assetProvider)
                .AsSingle()
                .NonLazy();
        }

        private void GameFactoryInstalls()
        {
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .FromInstance(_gameFactory)
                .AsSingle()
                .NonLazy();
        }

        public void InputServiceInstaller()
        {
            Container
                .Bind<IInputServices>()
                .To<InputServices>()
                .FromInstance(_inputServices)
                .AsSingle()
                .NonLazy();
        }

        private void AIMovementInstaller()
        {
            Container
                .Bind<IMovement>().WithId("AIMovement")
                .To<AIMovement>()
                .FromComponentInParents();
        }

        private void MovementIstanller()
        {
            Container
                .Bind<IMovement>().WithId("ActorMove")
                .To<ActorMove>()
                .FromComponentInParents();
        }

        private void GroundDetector()
        {
            Container
                .Bind<IGroundDetector>()
                .To<GroundDetector>()
                .FromComponentInParents();
        }
    }
}