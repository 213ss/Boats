using System.Collections.Generic;
using Infrastructure.AssetManagment;
using Logic.Boat;
using Logic.GoldLoot;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory : Factory, IGameFactory
    {
        private IAssets _assets;

        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public List<IProgressReader> ProgressReaders { get; } = new List<IProgressReader>();


        [Inject]
        public void Construct(IAssets assets)
        {
            _assets = assets;
        }

        public GameObject CreateGameObject(string path)
        {
            GameObject actor = _assets.Instantiate(path);

            AddToProgressSaveded(actor);

            return actor;
        }

        public GameObject CreateGameObject(string path, Vector3 position)
        {
            GameObject actor = _assets.Instantiate(path, position);
            
            AddToProgressSaveded(actor);

            return actor;
        }

        public GoldLoots CreateGoldLoot(Vector3 position)
        {
            GameObject loot = _assets.Instantiate(AssetsPath.GoldLootPath, position);
            return loot.GetComponentInChildren<GoldLoots>();
        }

        public BaseBoat CreateBoat(Vector3 position)
        {
            GameObject boat = _assets.Instantiate(AssetsPath.Boat, position);
            return boat.GetComponent<BaseBoat>();
        }

        public void Cleanup()
        {
            ProgressWriters.Clear();
            ProgressReaders.Clear();
        }

        private void AddToProgressSaveded(GameObject target)
        {
            foreach (IProgressReader progress in target.GetComponentsInChildren<IProgressReader>())
            {
                if (progress is ISavedProgress savedProgress)
                    ProgressWriters.Add(savedProgress);

                ProgressReaders.Add(progress);
            }
        }
    }
}