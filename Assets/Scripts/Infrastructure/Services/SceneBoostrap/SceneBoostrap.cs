using Infrastructure.LevelStates;
using Infrastructure.LevelStates.States;
using Infrastructure.Services.Game;
using Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Services.SceneBoostrap
{
    public class SceneBoostrap : MonoBehaviour, ICoroutineRunner
    {
        [Inject] private DiContainer _diContainer;
        
        private StateMachine _stateMachine;

        private void Awake()
        {
            GameService gameService = FindObjectOfType<GameService>();
            if (gameService == null)
            {
                SceneManager.LoadScene(0);
                return;
            }
            
            _stateMachine = new StateMachine(_diContainer, this);
            
            _stateMachine.Enter<LoadLevelState>();
        }
    }
}