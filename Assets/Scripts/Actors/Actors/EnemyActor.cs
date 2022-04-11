using Damageable;
using Infrastructure.Services.Girls;
using Infrastructure.Services.GoldLoot;
using Infrastructure.Services.GroundDetector;
using Infrastructure.Services.LevelService;
using Logic.AI;
using Logic.UI.GoldWidget;
using Logic.Weapon;
using UnityEngine;
using Zenject;

namespace Actors.Actors
{
    public class EnemyActor : Actor, IApplyForce
    {
        public PlayerActor PlayerActor => _playerActor;
        
        [SerializeField] private GoldCountIndicator _goldIndicator;
        [SerializeField] private Weapon _weaponShovel;
        
        [Inject(Id = "AIMovement")] private IMovement _movement;

        private IGroundDetector _groundDetector;
        private bool _isGrounded = true;

        private PlayerActor _playerActor;
        private Rigidbody _rigidbody;
        private AiBrain _aiBrain;

        private bool _isAttacked;

        private StickmanAnimator _stickmanAnimator;
        private IGoldLootService _goldLootService;
        private IGirlsService _girlService;

        [Inject]
        private void Construct(IGroundDetector groundDetector, 
            IGoldLootService goldLootService, 
            ILevelService levelService, IGirlsService girlsService)
        {
            _playerActor = levelService.PlayerActor as PlayerActor;
            
            _groundDetector = groundDetector;
            _groundDetector.OnGrounded += OnGrounded;
            _goldLootService = goldLootService;

            _girlService = girlsService;
        }

        private void Start()
        {
            _aiBrain = GetComponent<AiBrain>();
            _rigidbody = GetComponent<Rigidbody>();
            
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.isKinematic = true;
                _rigidbody.freezeRotation = true;
            }

            _stickmanAnimator = GetComponentInChildren<StickmanAnimator>();
        }

        public override void YouWin()
        {
            if (_goldIndicator != null)
                _goldIndicator.DisableIndicator();
            
            int randomIndex = Random.Range(0, 3);
            _girlService.StartGirlDancing(randomIndex);
            _stickmanAnimator.PlayWinner(randomIndex);
            
            base.YouWin();
            _movement.DisableMoveComponent();
            _groundDetector.DisableGroundChecker();
            _weaponShovel.enabled = false;
            _aiBrain.DisableAI();
        }

        public override void EnableActor()
        {
            _aiBrain.EnableAI();
        }

        public override void DisableActor()
        {
            _aiBrain.DisableAI();
        }

        public void ApplyForce(Vector3 force)
        {
            if(_isGrounded == false) return;
            if(_isAttacked) return;

            _isAttacked = true;
            IsAttacked = true;

            _goldLootService.OnDropGold(GoldService, ActorTransform.position, owner: this);
            _aiBrain.DisableAI();
            _movement.DisableMoveComponent();
            _rigidbody.isKinematic = false;

            _rigidbody.AddForce(force, ForceMode.Impulse);
            _groundDetector.EnableGroundChecker();
        }

        public override void DroppedOutGame()
        {
            base.DroppedOutGame();
            
            CurrentIsland.RemoveActorFromIsland(this);
            _rigidbody.isKinematic = false;
            _movement.DisableMoveComponent();
            _groundDetector.DisableGroundChecker();
            _aiBrain.DisableAI();
            GetComponent<AttackEnemy>().enabled = false;
        }

        private void OnGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;

            if (isGrounded && IsDroppedOutGame == false)
            {
                _rigidbody.isKinematic = true;
                _movement.EnableMoveComponent();
                
                _aiBrain.EnableAI();
                _groundDetector.DisableGroundChecker();
                _isAttacked = false;
                IsAttacked = false;
            }
        }
        

        private void OnDestroy()
        {
            _groundDetector.OnGrounded -= OnGrounded;
        }
    }
}
