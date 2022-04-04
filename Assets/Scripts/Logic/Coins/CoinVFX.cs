using UnityEngine;

namespace Logic.Coins
{
    public class CoinVFX : MonoBehaviour
    {
        public Transform ThisTransform => _transform;
        public Rigidbody ThisRigidBody => _rigidbody;
        
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;


        public void ShowCoin()
        {
            gameObject.SetActive(true);
        }

        public void AddForce(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public void EnablePhysics()
        {
            _rigidbody.isKinematic = false;
        }

        public void DisablePhysics()
        {
            _rigidbody.isKinematic = true;
        }

        public void HideCoin()
        {
            gameObject.SetActive(false);
        }
    }
}