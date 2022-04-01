using Newtonsoft.Json;
using UnityEngine;

namespace DungeonMaster.Code.Storage
{
    public class PersistentStorage: MonoBehaviour
    {
        public void Load<T>(T storageObject, string customName = "")
            where T : StorageObject
        {
            var prefName = customName.Length > 0 ? customName : typeof(T).Name;
            
            if (PlayerPrefs.HasKey(prefName))
            {
                var json = PlayerPrefs.GetString(prefName);
                JsonConvert.PopulateObject(json, storageObject);
            }
            else
            {
                storageObject.LoadDefaults();
            }
            storageObject.OnLoaded();
        }

        public void Save<T>(T storageObject, string customName = "")
            where T : StorageObject
        {
            var prefName = customName.Length > 0 ? customName : storageObject.GetType().Name;

            storageObject.OnSaved();
            var json = JsonConvert.SerializeObject(storageObject);
            PlayerPrefs.SetString(prefName, json);
        }
    }
}