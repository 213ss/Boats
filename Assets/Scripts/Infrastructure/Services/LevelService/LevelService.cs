using Actors;
using Infrastructure.Data.ScriptableObjects;
using Infrastructure.Services.Game;
using UnityEngine;

namespace Infrastructure.Services.LevelService
{
    public class LevelService : MonoBehaviour, ILevelService
    {
        public int MaxLevelsCount => _gameService.ScenesCount;
        public int CurrentSceneIndex => _gameService.CurrentSceneIndex;
        
        public Actor PlayerActor => _playerActor;
        public AiData AIParametersData => _aiParametersData;
        
        [SerializeField] private AiData _aiParametersData;
        
        private Actor _playerActor;

        private GameService _gameService;
        

        public void SetPlayerActor(Actor actor)
        {
            _playerActor = actor;
        }

        public void SetGameService(GameService gameService)
        {
            _gameService = gameService;
        }
        
        public void RestartLevel()
        {
            _gameService.RestartLevel();
        }

        public void LoadNextLevel()
        {
            _gameService.LoadNextLevel();
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