﻿using System.Collections.Generic;
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
            string[] names = new string[_sceneNames.Count];

            for (int i = 0; i < _scenes.Count; i++)
            {
                names[i] = _scenes[i].name;
            }

            _levelReferences.SetSceneNames(names);
        }
#endif
    }
}