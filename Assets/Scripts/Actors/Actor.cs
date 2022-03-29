using System;
using Logic.Island;
using Scripts.Infrastructure.Data;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;
using Zenject;

namespace Actors
{
    public abstract class Actor : MonoBehaviour
    {
        public event Action OnDroppedOutGame;
        public bool IsTravel { get; set; }
        public bool IsHoleStay { get; set; }
        public bool IsWin => _isWin;
        public bool IsAttacked { get; protected set; }
        public float ApproachDistance => _approachDistance;
        
        public string ActorName => _actorName;
        public Team ActorTeam => _actorTeam;
        public Transform ActorTransform => _actorTransform;
        public bool IsDroppedOutGame => _isDroppedOutGame;
        public BaseIsland CurrentIsland => _currentIsland;

        public IGoldChanger GoldService => _goldService;
        
        [SerializeField] protected string _actorName;
        [SerializeField] protected Team _actorTeam;
        [SerializeField] protected float _approachDistance;
        
        
        [Header("Components")]
        [SerializeField] protected Transform _actorTransform;
        
        
        private bool _isDroppedOutGame;
        private bool _isWin;
        
        private IGoldChanger _goldService;
        private BaseIsland _currentIsland;

        
        [Inject]
        private void Construction(IGoldChanger _gold)
        {
            _goldService = _gold;
        }

        public virtual void EnableActor()
        {
            
        }

        public virtual void DisableActor()
        {
            
        }

        public void SetCurrentIsland(BaseIsland island)
        {
            _currentIsland = island;
        }

        public virtual void YouWin()
        {
            _isWin = true;
        }

        public void SetTeam(Team team)
        {
            _actorTeam = team;
        }
        
        public virtual void DroppedOutGame()
        {
            _isDroppedOutGame = true;
            OnDroppedOutGame?.Invoke();
        }


    }
}
