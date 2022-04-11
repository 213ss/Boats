using System.Collections;
using Actors;
using Scripts.Infrastructure;
using UnityEngine;

namespace Infrastructure.LevelStates.States
{
    public class LevelProcessState : ILevelState
    {
        private readonly StateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;

        private Actor _playerActor;
        private Coroutine _playerConditionCoroutine;

        public LevelProcessState(StateMachine stateMachine, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
        }

        public void Enter()
        {
            FindPlayerActor();
            StartCheckLevelState();
            
            EnableAllActors();
        }

        public void Exit()
        {
            _coroutineRunner.StopCoroutine(_playerConditionCoroutine);
            DisableAllActors();
        }

        private void FindPlayerActor()
        {
            _playerActor = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
        }

        private void EnableAllActors()
        {
            var allActorInScene = GameObject.FindObjectsOfType<Actor>();
            for (int i = 0; i < allActorInScene.Length; i++)
            {
                allActorInScene[i].EnableActor();
            }
        }

        private void DisableAllActors()
        {
            var allActorInScene = GameObject.FindObjectsOfType<Actor>();
            for (int i = 0; i < allActorInScene.Length; i++)
            {
                allActorInScene[i].DisableActor();
            }
        }

        private void StartCheckLevelState()
        {
            _playerConditionCoroutine = _coroutineRunner.StartCoroutine(PlayerCondition());
        }

        private IEnumerator PlayerCondition()
        {
            while (true)
            {
                if (_playerActor.IsDroppedOutGame)
                {
                    _stateMachine.Enter<LevelLoseState>();
                }

                if (_playerActor.IsWin)
                {
                    _stateMachine.Enter<LevelWinState>();                    
                }

                if (_playerActor.CurrentIsland.GetFreeBoat() == null && _playerActor.CurrentIsland.IsLast == false)
                    _playerActor.DroppedOutGame();
                
                yield return null;
            }
        }
    }
}