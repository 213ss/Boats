using Infrastructure.Data;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(CharacterController))]
    public class ActorMove : MonoBehaviour, ISavedProgress, IMovement
    {
        public bool IsMovement { get; private set; }
        public bool IsDisableMovement { get; private set; }
        public bool IsStopped => _isStopped;
        
        [SerializeField] private float _moveSpeed;

        [Header("Component references")]
        [SerializeField] private CharacterController _characterController;


        private StickmanAnimator _stickmanAnimator;
        private bool _isStopped;
        
        
        private void Start()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();

            _stickmanAnimator = GetComponentInChildren<StickmanAnimator>();
            
            DisableMoveComponent();
        }
        

        public void Move(Vector3 direction)
        {
            if(IsDisableMovement) return;
            
            if (direction != Vector3.zero)
            {
                IsMovement = true;
                _isStopped = false;
                //_characterController.SimpleMove(direction * _moveSpeed);
                transform.forward = direction;
                
                _stickmanAnimator.StopDigging();
                _stickmanAnimator.Move();
            }
            
            _characterController.SimpleMove(direction * _moveSpeed);
            
            if(direction == Vector3.zero)
            {
                IsMovement = false;
                _isStopped = true;
                _stickmanAnimator.StopMoving();
            }
        }

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }

        public void EnableMoveComponent()
        {
            _characterController.enabled = true;
            IsDisableMovement = false;
        }

        public void DisableMoveComponent()
        {
            _stickmanAnimator.StopMoving();
            IsDisableMovement = true;
            _isStopped = true;
            _characterController.enabled = false;
        }


        public void LoadProgress(PlayerProgress progress)
        {
            
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            
        }
        
    }
}