using System.Collections.Generic;
using Actors;
using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Infrastructure.Services.Islands;
using Infrastructure.Services.UIDirect;
using Logic.Boat;
using Logic.GoldLoot;
using Logic.Triggers;
using Scripts.Infrastructure.Data;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Logic.Island
{
    public class BaseIsland : MonoBehaviour
    {
        public bool IsLast => _isLastIsland;
        public Transform GetFinalPoint
        {
            get { return _finalPoint; }
        }

        public int CostDelivery => _costDelivery;
        public Vector3 PierPosition => _boatsPier.position;

        [Header("Trigger parameters")]
        [SerializeField] private int _countTriggers;
        [SerializeField] private int _goldTriggerPrize;

        [Header("Destination point")]
        [SerializeField] private int _costDelivery;
        [SerializeField] private Transform _destinationPoint;
        
        [SerializeField] private Transform _boatsPier;

        [Header("If this Island is last?")] 
        [SerializeField] private bool _isLastIsland;

        [SerializeField] private Transform _finalPoint;

        [Header("Boats spawn, max count equal Boats Points length")] 
        [SerializeField] private int _boatsCount;
        [SerializeField] private Transform[] _boatsPoints;
        [SerializeField] private UIDirectToWorldObject[] _indicators;
        
        private IGameFactory _gameFactory;
        private IIslandService _islandService;

        [Header("Trigger settings")] 
        [SerializeField] private float _distanceSpawn;
        [SerializeField] private Transform _spawnTriggerTransform;

        private List<Actor> _actorsInInsland = new List<Actor>();
        private List<Actor> _actorsDelivry = new List<Actor>();
        private List<GoldLoots> _goldLoots = new List<GoldLoots>();
        private BaseBoat[] _boats;
        
        private GoldAreaTrigger[] _allGoldTriggers;


        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _gameFactory = diContainer.Resolve<IGameFactory>();
            _islandService = diContainer.Resolve<IIslandService>();
        }
        
        private void Awake()
        {
            if(_spawnTriggerTransform == null)
                _spawnTriggerTransform = GetComponent<Transform>();
            
            GenerateBoats();
            GenerateGoldTriggersOnIsland();
        }

        public bool DeliveryActor(Actor actor)
        {
            if (_actorsDelivry.Contains(actor))
                return true;
            
            IBoat boat = GetFreeBoat();
            
            if (boat == null) return false;

            var successDelivery = boat.TryStartDelivery(actor);

            if (successDelivery)
            {
                _actorsDelivry.Add(actor);

                if (actor.ActorTeam == Team.Player_0)
                {
                    HideIndicators();
                }
                else
                {
                    boat.HideIndicator();
                }
            }

            return successDelivery;
        }

        public void AddGoldLoots(GoldLoots goldLoot)
        {
            _goldLoots.Add(goldLoot);
        }

        public GoldLoots[] GetGoldLoots()
        {
            ClearNullGoldLoots();
            if (_goldLoots.Count == 0)
                return null;
            
            return _goldLoots.ToArray();
        }

        public int GetCountGoldLoots()
        {
            ClearNullGoldLoots();
            return _goldLoots.Count;
        }

        public void ShowIndicators()
        {
            for (int i = 0; i < _boats.Length; i++)
            {
                _boats[i].ShowIndicator();
            }
        }

        public void HideIndicators()
        {
            for (int i = 0; i < _boats.Length; i++)
            {
                _boats[i].HideIndicator();
            }
        }

        private void ClearNullGoldLoots()
        {
            for (int i = 0; i < _goldLoots.Count; ++i)
            {
                if(_goldLoots[i] == null) _goldLoots.RemoveAt(i);
            }
        }

        public Vector3 GetRandomPointInIsland()
        {
            if (_isLastIsland) return _finalPoint.position;

            Vector3 randomPosition = Random.insideUnitSphere * _distanceSpawn;
            randomPosition += _spawnTriggerTransform.position;
            randomPosition.y = 0.5f;
            
            
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(randomPosition, out navMeshHit, _distanceSpawn, 3);
            return navMeshHit.position;
        }

        public void AddActorToIsland(Actor actor)
        {
            _actorsInInsland.Add(actor);
        }

        public int GetCountActorExistGold()
        {
            int count = 0;
            for (int i = 0; i < _actorsInInsland.Count; ++i)
            {
                if (_actorsInInsland[i].GoldService.CurrentCount >= 1)
                    count++;
            }

            return count;
        }

        public void RemoveActorFromIsland(Actor actor)
        {
            _actorsInInsland.Remove(actor);
        }

        public bool ActorExistOnIsland(Actor actor)
        {
            return _actorsInInsland.Contains(actor);
        }

        public Vector3 GetPointRandomGoldTrigger()
        {
            List<Vector3> position = new List<Vector3>();

            for (int i = 0; i < _allGoldTriggers.Length; i++)
            {
                if (_allGoldTriggers[i].IsCooldown == false)
                {
                    position.Add(_allGoldTriggers[i].ThisTransform.position);
                }
            }

            if (position.Count == 0) return new Vector3(-19.0f, -19.0f, -19.0f);

            if (position.Count == 1)
                return position[0];
            
            
            return position[Random.Range(0, position.Count - 1)];
        }

        public IBoat GetFreeBoat()
        {
            for (int i = 0; i < _boats.Length; i++)
            {
                if (_boats[i].IsEmpty)
                    return _boats[i];
            }

            return null;
        }

        private void StartDeliveryActor(Actor passenger)
        {
            _islandService.SetActorToNextIsland(passenger);
        }

        public int GetCountActiveTriggers()
        {
            int count = 0;
            foreach (var goldTrigger in _allGoldTriggers)
            {
                if (goldTrigger.IsCooldown == false)
                    count++;
            }

            return count;
        }

        private void DropOffActor(Actor passenger)
        {
            if(passenger.ActorTeam == Team.Player_0)
                passenger.CurrentIsland.ShowIndicators();
        }

        private void GenerateBoats()
        {
            _boats = new BaseBoat[_boatsCount];
            
            for (int i = 0; i < _boatsCount; i++)
            {
                if(i > _boatsCount) return;

                _boats[i] = _gameFactory.CreateBoat(_boatsPoints[i].position);
                _boats[i].transform.rotation = _boatsPoints[i].rotation;
                _boats[i].transform.SetParent(_boatsPoints[i]);
                _boats[i].SetIndicator(_indicators[i]);
                
                _boats[i].SetCostDelivery(_costDelivery);
                _boats[i].SetDestinationPoint(_destinationPoint);
                
                _boats[i].OnStartDelivery += StartDeliveryActor;
                _boats[i].OnDropOff += DropOffActor;
            }
            
        }

        private void GenerateGoldTriggersOnIsland()
        {
            _allGoldTriggers = new GoldAreaTrigger[_countTriggers];
            
            for (int i = 0; i < _countTriggers; ++i)
            {
                GameObject goldTrigger =
                    _gameFactory.CreateGameObject(AssetsPath.GoldTrigger, GetRandomPointInIsland());
                
                goldTrigger.transform.SetParent(_spawnTriggerTransform);

                _allGoldTriggers[i] = goldTrigger.GetComponent<GoldAreaTrigger>();
                _allGoldTriggers[i].SetGoldPrize(_goldTriggerPrize);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _boats.Length; ++i)
            {
                _boats[i].OnStartDelivery -= StartDeliveryActor;
                _boats[i].OnDropOff -= DropOffActor;
            }
        }

#if UNITY_EDITOR

        [Header("Only editor mode")]
        public Color GizmoColor;

        public float PointSize;

        public bool IsDisplaying = true;

        private void OnDrawGizmos()
        {
            if (IsDisplaying)
            {
                Gizmos.color = GizmoColor;

                if (_boatsPier != null)
                {
                    Gizmos.DrawSphere(_boatsPier.position, PointSize);

                    if (_boatsPier != null && _destinationPoint != null)
                    {
                        Gizmos.DrawSphere(_destinationPoint.position, PointSize);
                        Gizmos.DrawLine(_boatsPier.position, _destinationPoint.position);
                    }
                }

                if(_boatsPoints != null && _boatsPoints.Length > 0)
                    foreach (var boatsPoint in _boatsPoints)
                    {
                        Gizmos.DrawSphere(boatsPoint.position, 0.3f);
                    }

            }
        }


#endif

    }
}
