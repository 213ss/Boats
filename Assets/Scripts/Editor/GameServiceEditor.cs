using Infrastructure.Data.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelsData))]
    public class GameServiceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelsData levelsData = (LevelsData)target;

            if (GUILayout.Button("Set levels"))
            {
                levelsData.SaveLevelNames();
            }
            
            EditorUtility.SetDirty(levelsData);
            
        }
    }
}