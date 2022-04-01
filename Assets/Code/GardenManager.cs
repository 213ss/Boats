using System;
using System.Linq;
using Code.Models;
using Code.Storage.StorageObjectImpls;
using DungeonMaster.Code.Storage;
using UnityEngine;

namespace Code
{
    public class GardenManager : MonoBehaviour
    {
        public const string GardenStorageKey = "GardenStorage";
        
        [SerializeField] private PersistentStorage storage;
        
        [field: SerializeField]
        public GardenObjectModel[] GardenObjectModels { get; private set; }
        
        private void Awake()
        {
           storage.Load(new GardenStorageObject(this), GardenStorageKey);
        }

        public void HideAll()
        {
            var gardenObjectViews = GetComponentsInChildren<GardenObjectView>();

            foreach (var view in gardenObjectViews)
            {
                view.gameObject.SetActive(false);
            }
        }
        
        public void Init(GardenObjectModel[] models)
        {
            GardenObjectModels = models;
            var gardenObjectViews = GetComponentsInChildren<GardenObjectView>();
            var typeToView = gardenObjectViews.ToDictionary(view => view.Type);
            
            foreach (var model in models)
            {
                if (typeToView.TryGetValue(model.Type, out var view))
                {
                    model.Cost = view.Cost;
                    view.Init(model);
                }
            }
            
            storage.Save(new GardenStorageObject(this), GardenStorageKey);
        }

        [ContextMenu("Fill")]
        private void Fill()
        {
            var gardenObjectViews = GetComponentsInChildren<GardenObjectView>();
            var typeToView = gardenObjectViews.ToDictionary(view => view.Type);
            
            GardenObjectModels = Enum.GetValues(typeof(GurdenObjectEnum))
                .Cast<GurdenObjectEnum>()
                .Select(value =>
                {
                    var obj =  new GardenObjectModel()
                    {
                        Type = value,
                        IsCollected = false
                    };
                    
                    if (typeToView.TryGetValue(obj.Type, out var view))
                    {
                        obj.Cost = view.Cost;
                    }

                    return obj;
                })
                .ToArray();
        }
    }
}