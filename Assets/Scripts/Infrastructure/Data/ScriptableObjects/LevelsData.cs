using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Levels data", fileName = "LevelsData")]
    public class LevelsData : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private LevelReferences _levelReferences;
        
        [SerializeField] private List<SceneAsset> _scenes = new List<SceneAsset>();

        private List<string> _sceneNames = new List<string>();
        
        public void SaveLevelNames()
        {
            StreamWriter writer;
            FileInfo file = new FileInfo("Assets/Resources/Data/LevelsData.csv");
            writer = file.CreateText();
            
            for (int i = 0; i < _scenes.Count; i++)
            {
                writer.Write(_scenes[i].name + ",");
            }

            writer.Close();
        }
        
#endif
    }
}