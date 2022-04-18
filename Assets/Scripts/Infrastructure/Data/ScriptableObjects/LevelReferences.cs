using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Level references", fileName = "LevelReferences")]
    public class LevelReferences : ScriptableObject
    {
        [SerializeField] private TextAsset _levelNamesText;
        
        private List<string> _sceneNames = new List<string>();
        

        public string[] GetSceneNames()
        {
            string text = _levelNamesText.text;

            string[] allNames = text.Split(',');

            List<string> names = new List<string>();

            for (int i = 0; i < allNames.Length; ++i)
            {
                if (allNames[i] != "")
                {
                    names.Add(allNames[i]);
                }
            }

            return names.ToArray();
        }
    }
}