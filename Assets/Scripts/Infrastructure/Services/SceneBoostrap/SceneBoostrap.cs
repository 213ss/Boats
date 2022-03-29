using Infrastructure.LevelStates;
using Infrastructure.LevelStates.States;
using Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.SceneBoostrap
{
    public class SceneBoostrap : MonoBehaviour, ICoroutineRunner
    {
        [Inject] private DiContainer _diContainer;
        
        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new StateMachine(_diContainer, this);
            
            _stateMachine.Enter<LoadLevelState>();
        }
    }
}