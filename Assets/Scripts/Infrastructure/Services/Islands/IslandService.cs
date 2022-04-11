using System;
using Actors;
using Logic.Island;
using Scripts.Infrastructure.Data;
using UnityEngine;

namespace Infrastructure.Services.Islands
{
    public class IslandService : MonoBehaviour, IIslandService
    {
        public event Action<int> EventActorNewIsland;

        [SerializeField] private BaseIsland[] _allIslands;

        private void Awake()
        {
            if (_allIslands == null || _allIslands.Length == 0)
                _allIslands = FindObjectsOfType<BaseIsland>();
        }

        private void Start()
        {
            _allIslands[0].ShowIndicators();
        }

        public void SetActorToStartIsland(Actor actor)
        {
            _allIslands[0].AddActorToIsland(actor);
            actor.SetCurrentIsland(_allIslands[0]);
        }

        public int GetCountIslands()
        {
            return _allIslands.Length;
        }
        

        public void SetActorToNextIsland(Actor actor)
        {
            var currentIslandIndex = GetIslandIndex(actor);
            currentIslandIndex++;
            
            if (currentIslandIndex < _allIslands.Length)
            {
                _allIslands[--currentIslandIndex].RemoveActorFromIsland(actor);
                _allIslands[++currentIslandIndex].AddActorToIsland(actor);
                actor.SetCurrentIsland(_allIslands[currentIslandIndex]);
                
                if(actor.ActorTeam == Team.Player_0)
                    EventActorNewIsland?.Invoke(currentIslandIndex);
            }
        }

        public void RemoveActorFromIsland(Actor actor)
        {
            BaseIsland island = TryGetIslandForActor(actor);
            island.RemoveActorFromIsland(actor);
        }

        public BaseIsland TryGetIslandForActor(Actor actor)
        {
            foreach (var island in _allIslands)
            {
                if (island.ActorExistOnIsland(actor))
                {
                    return island;
                }
            }

            return null;
        }

        private int GetIslandIndex(Actor actor)
        {
            for (int i = 0; i < _allIslands.Length; i++)
            {
                if (_allIslands[i].ActorExistOnIsland(actor))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
