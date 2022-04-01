using System;
using Code.Models;
using Code.Runner;
using DungeonMaster.Code.Storage;
using Newtonsoft.Json;

namespace Code.Storage.StorageObjectImpls
{
    public class GateStorageObject : StorageObject
    {
        private readonly GateManager _gateManager;
        [JsonProperty("GardenObjects")] private GardenObjectModel[] _objects;

        public GateStorageObject(GateManager gateManager)
        {
            _gateManager = gateManager;
        }


        public override void LoadDefaults()
        {
            throw new Exception("Load garden level first");
        }

        public override void OnLoaded()
        {
            _gateManager.Init(_objects);
        }

        public override void OnSaved()
        {
            _objects = _gateManager.GardenObjectModels;
        }
    }
}