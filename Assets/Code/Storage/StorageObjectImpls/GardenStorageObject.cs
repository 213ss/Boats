using System;
using System.Linq;
using Code.Models;
using DungeonMaster.Code.Storage;
using Newtonsoft.Json;

namespace Code.Storage.StorageObjectImpls
{
    [Serializable]
    public class GardenStorageObject: StorageObject
    {
        [JsonIgnore] private readonly GardenManager _gardenManager;
        
        [JsonProperty("GardenObjects")] private GardenObjectModel[] _objects;

        public GardenStorageObject(GardenManager gardenManager)
        {
            _gardenManager = gardenManager;
        }
        
        public override void LoadDefaults()
        {
            _objects = Enum.GetValues(typeof(GurdenObjectEnum))
                .Cast<GurdenObjectEnum>()
                .Select(value => new GardenObjectModel()
                {
                    Type = value,
                    IsCollected = false,
                    IsNew = true
                })
                .ToArray();
        }

        public override void OnLoaded()
        {
            _gardenManager.Init(_objects);
        }

        public override void OnSaved()
        {
            _objects = _gardenManager.GardenObjectModels;
        }
    }
}