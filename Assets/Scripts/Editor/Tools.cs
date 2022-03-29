using Logic.Triggers;
using UnityEditor;
using UnityEngine;

public static class Tools
{
    [MenuItem("Tools/Clear prefs")]
    public static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    
    [MenuItem("Tools/Show or hide all triggers")]
    public static void ShowOrHideAllTriggers()
    {
        var triggers = GameObject.FindObjectsOfType<GoldAreaTrigger>();
        foreach (var trigger in triggers)
        {
            //trigger.EnableOrDisableView();
        }
    }
}
