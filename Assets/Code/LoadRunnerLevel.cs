using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class LoadRunnerLevel : MonoBehaviour
    {
        

        public void LoadScene()
        {

            int ind = PlayerPrefs.GetInt("lvl", 2);

            if (ind > SceneManager.sceneCountInBuildSettings - 1) 
                ind = 2;
            SceneManager.LoadScene(ind);
        }
    }
}