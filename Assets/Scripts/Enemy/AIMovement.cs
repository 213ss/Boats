using System;
using Actors;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovement : MonoBehaviour, IAIMovement
    {
        public bool IsMovement { get; private set; }
        public bool IsStopped { get; private set; }
        public bool IsDisableMovement { get; private set; }

        [SerializeField] private NavMeshAgent _agent;
        
        private StickmanAnimator _stickmanAnimator;
        private Transform _ownerTransform;
        private Actor _ownerActor;

        private Vector3 _destination;
        
        private void Start()
        {
            _ownerActor = GetComponent<Actor>();
            
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }

            _ownerTransform = GetComponent<Transform>();

            _stickmanAnimator = GetComponentInChildren<StickmanAnimator>();
        }

        private void Update()
        {
            if (IsMovement == false)
                return;

            if ((_ownerTransform.position - _destination).magnitude <= _agent.stoppingDistance)
            {
                IsMovement = false;
                _stickmanAnimator.StopMoving();
            }

        }

        public void SetDestination(Vector3 point)
        {
            if (IsDisableMovement == false)
            {
                IsMovement = true;
                _destination = point;
                _destination.y = _ownerTransform.position.y;
                _stickmanAnimator.Move();
                _agent.SetDestination(_destination);
            }
        }

        public bool IsAFK()
        {
            if (IsDisableMovement == false && IsMovement && _agent.velocity.magnitude <= 1.0f)
            {
                return true;
            }

            return false;
        }

        public float GetRemainingDistance()
        {
            if(IsDisableMovement == false)
                return _agent.remainingDistance;

            return 0.0f;
        }

        public void Move(Vector3 direction)
        {
            throw new NotImplementedException();
        }

        public void SetMoveSpeed(float speed)
        {
            _agent.speed = speed;
        }

        public void EnableMoveComponent()
        {
            _agent.enabled = true;
            IsDisableMovement = false;
            IsStopped = false;

            if (IsMovement)
            {
                SetDestination(_destination);
            }
        }

        public void DisableMoveComponent()
        {
            if (_ownerActor.IsTravel)
            {
                IsMovement = false;
            }
            
            _stickmanAnimator.StopMoving();
            IsDisableMovement = true;
            IsStopped = true;
            _agent.enabled = false;
        }

        public void Repath()
        {
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawSphere(_destination, 0.2f);
            Gizmos.DrawLine(_ownerTransform.position, _destination);
        }
    }
}
