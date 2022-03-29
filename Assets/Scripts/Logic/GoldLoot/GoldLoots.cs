using Actors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.GoldLoot
{
    public class GoldLoots : MonoBehaviour
    {
        [SerializeField] private float _forcePower;
        [SerializeField] private Rigidbody _rigidbody;

        private float _goldCount;

        private float _cooldownTime = 1.0f;
        private bool _isCooldown = true;

        private void Update()
        {
            if (_isCooldown)
            {
                if (_cooldownTime <= 0.0f)
                    _isCooldown = false;
                
                _cooldownTime -= Time.deltaTime;
            }
        }

        public void SetGold(float amount)
        {
            _goldCount = amount;
        }

        public void AddForce()
        {
            Vector3 randomDirection = new Vector3(Random.Range(-2.0f, 2.0f), 0.0f,
                Random.Range(-2.0f, 2.0f));
            
            Vector3 force = (Vector3.up + randomDirection) * _forcePower;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_isCooldown) return;
            
            if (other.TryGetComponent<Actor>(out var actor))
            {
                actor.GoldService.AddGold(_goldCount);
                Destroy(_rigidbody.gameObject);
            }
        }
        
    }
}