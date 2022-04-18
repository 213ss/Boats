using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Level references", fileName = "LevelReferences")]
    public class LevelReferences : ScriptableObject
    {
        private List<string> _sceneNames = new List<string>();

        public void SetSceneNames(string[] names)
        {
            _sceneNames.Clear();
            _sceneNames.AddRange(names);
        }

        public string[] GetSceneNames()
        {
            return _sceneNames.ToArray();
        }
    }
}