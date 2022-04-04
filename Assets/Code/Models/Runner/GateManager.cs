using System;
using System.Linq;
using Code.Models;
using Code.Storage.StorageObjectImpls;
using Code.Utils;
using DungeonMaster.Code.Storage;
using Runner;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Runner
{
    public class GateManager : MonoBehaviour
    {
        [SerializeField] public bool generateLevel;
        [SerializeField] private Gate gatePrefab;
        [SerializeField] private PersistentStorage storage;

        public bool GenerateLevel => generateLevel;

        public GardenObjectModel[] GardenObjectModels { get; private set; }

        private void Awake()
        {
            storage.Load(new GateStorageObject(this), GardenManager.GardenStorageKey);
        }

        public void Init(GardenObjectModel[] gardenObjectModels)
        {
            GardenObjectModels = gardenObjectModels;
            
            if(generateLevel)
                return;

            var gates = GetComponentsInChildren<Gate>();
            foreach (var gate in gates)
            {
                gate.Init(this, null);
            }
        }

        private Gate CreateGate(Vector3 position, Transform parent)
        {
            var model = GardenObjectModels.Where(model => !model.IsCollected).ChooseOne();
            var instance = Instantiate(gatePrefab.gameObject, parent).GetComponent<Gate>();
            instance.transform.position = position;
            instance.Init(this, model);
            return instance;
        }

        public void Collect(GurdenObjectEnum type)
        {
            var model = GardenObjectModels.FirstOrDefault(m => m.Type == type);
            if (model != null)
            {
                model.IsCollected = true;
            }
            storage.Save(new GateStorageObject(this), GardenManager.GardenStorageKey);
        }
    }
}