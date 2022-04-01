using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GoToGardenButton : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(PlayerPrefs.GetInt("lvl",2) > 2);
        }

        public void GotoGarden()
        {
            SceneManager.LoadScene(1);
        }
    }
}