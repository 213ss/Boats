using Actors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.LevelService
{
    public class LevelService : MonoBehaviour, ILevelService
    {
        public Actor PlayerActor => _playerActor;

        private Actor _playerActor;

        public void SetPlayerActor(Actor actor)
        {
            _playerActor = actor;
        }
        
        public void RestartLevel()
        {
            SceneManager.LoadScene(1);
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