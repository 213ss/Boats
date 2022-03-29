using Actors;
using Infrastructure.Factory;
using Infrastructure.Services.Islands;
using Logic.GoldLoot;
using Logic.Island;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.GoldLoot
{
    public class GoldLootService : MonoBehaviour, IGoldLootService
    {
        [SerializeField] private float _percentDrop;
        [SerializeField] private int _shardCount;

        private IGameFactory _gameFactory;
        private IIslandService _islandService;

        [Inject]
        private void Construct(IGameFactory gameFactory, DiContainer diContainer)
        {
            _gameFactory = gameFactory;
            _islandService = diContainer.Resolve<IIslandService>();
        }


        public void OnDropGold(IGoldChanger goldChanger, Vector3 spawnPosition, Actor owner)
        {
            float gold = goldChanger.CurrentCount;
            float percent = (gold / 100.0f) * _percentDrop;
            
            if(gold <= 0.0f) return;
            
            BaseIsland island = _islandService.TryGetIslandForActor(owner);

            
            if (percent <= _shardCount)
            {
                goldChanger.SubstractionGold(gold);
                GoldLoots loots = _gameFactory.CreateGoldLoot(spawnPosition);

                island.AddGoldLoots(loots);
                loots.SetGold(gold);
                loots.AddForce();
                return;
            }
            
            goldChanger.SubstractionGold(percent);
            
            float goldPerShard = percent / _shardCount;

            for (int i = 0; i < _shardCount; ++i)
            {
                GoldLoots loots = _gameFactory.CreateGoldLoot(spawnPosition);

                island.AddGoldLoots(loots);
                loots.SetGold(goldPerShard);
                loots.AddForce();
            }
        }
    }
}