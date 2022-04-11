using System;
using System.Collections;
using Actors;
using Infrastructure.Services.LevelService;
using Infrastructure.Services.UIDirect;
using Logic.MovementFinalPoint;
using Logic.UI.GoldWidget;
using Scripts.Infrastructure.Data;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Logic.Boat
{
    public class BaseBoat : MonoBehaviour, IBoat
    {
        public event Action<Actor> OnStartDelivery;
        public event Action<Actor> OnDropOff;

        public bool IsEmpty { get; private set; }
        
        [SerializeField] private float _costDelivery;
        [SerializeField] private float _dropOffsetDistance = 16.0f;
        [SerializeField] private Transform _passengerPoint;

        [Header("Maybe is null")] 
        [SerializeField] private NavMeshAgent _agent;

        [SerializeField] private Transform _thisTransform;
        [SerializeField] private GoldCountIndicator _goldCountIndicator;

        private UIDirectToWorldObject _indicator;
        private Actor _passenger;
        private Transform _destinationPoint;
        private IGoldChanger _gold;
        private ILevelService _levelService;

        [Inject]
        public void Construct(ILevelService levelService)
        {
            _levelService = levelService;
        }
        
        private void Awake()
        {
            _gold = _thisTransform.GetComponent<IGoldChanger>();
            _gold.AddGold(_costDelivery);
        }

        private void Start()
        {
            if (_thisTransform == null)
                _thisTransform = GetComponent<Transform>();

            if (_agent == null)
                _agent = GetComponent<NavMeshAgent>();

            if (_goldCountIndicator == null)
                _goldCountIndicator = GetComponent<GoldCountIndicator>();

            IsEmpty = true;
        }

        public void SetIndicator(UIDirectToWorldObject uiDirect)
        {
            _indicator = uiDirect;
        }

        public void SetCostDelivery(float cost)
        {
            _gold.SubstractionGold(_costDelivery);
            
            _costDelivery = cost;
            _gold.AddGold(_costDelivery);
        }

        public void SetDestinationPoint(Transform destination)
        {
            _destinationPoint = destination;
        }

        public bool TryStartDelivery(Actor passenger)
        {
            if (passenger.GoldService.CurrentCount < _costDelivery) return false;
            
            if (TrySetPassenger(passenger) == false) return false;

            IsEmpty = false;
            StartCoroutine(Delivery());
            return true;
        }

        public void ShowIndicator()
        {
            _indicator.SetFollowingObject(_thisTransform);
            _indicator.EnableIndicator();
        }

        public void HideIndicator()
        {
            if (_passenger != null && _passenger.ActorTeam != Team.Player_0)
            {
                bool existPlayer = _passenger.CurrentIsland.ActorExistOnIsland(_levelService.PlayerActor);
                if (existPlayer)
                {
                    return;
                }
            }
                
            _indicator.DisableIndicator();
        }

        private bool TrySetPassenger(Actor passenger)
        {
            if (_passenger != null) return false;

            _passenger = passenger;
            return true;
        }

        private IEnumerator Delivery()
        {
            if (_passenger.ActorTeam == Team.Player_0)
                HideIndicator();
            
            _passenger.IsTravel = true;
            _goldCountIndicator.DisableIndicator();
            OnStartDelivery?.Invoke(_passenger);
            
            _passenger.GoldService.SubstractionGold(_costDelivery);
            
            var movementActor = _passenger.GetComponent<IMovement>();
            movementActor.DisableMoveComponent();
            
            _passenger.ActorTransform.SetParent(_passengerPoint);
            _passenger.ActorTransform.position = _passengerPoint.position;
            _passenger.ActorTransform.rotation = _destinationPoint.rotation;

            Vector3 direction;
            _agent.SetDestination(_destinationPoint.position);
            
            do
            {
                
                direction = _destinationPoint.position - _thisTransform.position;
                yield return null;

            } while (direction.magnitude > _dropOffsetDistance);
            
            DropOff();
        }

        private void DropOff()
        {
            _passenger.ActorTransform.parent = null;
            _passenger.ActorTransform.position = _destinationPoint.position;

            if (_passenger.CurrentIsland.IsLast)
            {
                _passenger.GetComponent<NavMeshMovement>().GoTo(_passenger.CurrentIsland.GetFinalPoint.position);
                OnDropOff?.Invoke(_passenger);
                return;
            }
            
            _passenger.GetComponent<IMovement>().EnableMoveComponent();

            OnDropOff?.Invoke(_passenger);

            _passenger.IsTravel = false;
            _passenger = null;

            StartCoroutine(SinkingBoat());
            GetComponentInChildren<GoldCountIndicator>().DisableIndicator();
        }

        private IEnumerator SinkingBoat()
        {
            _agent.isStopped = true;
            _agent.enabled = false;
            
            float timer = 3.0f;
            while (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                _thisTransform.position += Vector3.down * 2.2f * Time.deltaTime;
                
                yield return null;
            }
            
            Destroy(gameObject);
        }
        
    }
}
