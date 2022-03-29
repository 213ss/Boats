using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.LevelService
{
    public class LevelService : MonoBehaviour, ILevelService
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }

        public void GoToMainMenu()
        {
            
        }

        public void QuitGame()
        {
            Application.Quit();   
        }
    }
}