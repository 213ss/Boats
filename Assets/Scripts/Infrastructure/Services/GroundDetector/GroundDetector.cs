using System;
using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.GroundDetector
{
    public class GroundDetector : MonoBehaviour, IGroundDetector
    {
        public event Action<bool> OnGrounded;
        
        [SerializeField] private float _minDistanceToGround;
        [SerializeField] private LayerMask _groundLayer;

        [SerializeField] private Transform _ownerTransform;
        private Coroutine _groundCheckerCoroutine;
        

        public void EnableGroundChecker()
        {
            if (_groundCheckerCoroutine == null)
                _groundCheckerCoroutine = StartCoroutine(GroundChecker());
        }

        public void DisableGroundChecker()
        {
            if (_groundCheckerCoroutine != null)
            {
                StopCoroutine(_groundCheckerCoroutine);
                _groundCheckerCoroutine = null;
            }
        }
        
        private IEnumerator GroundChecker()
        {
            yield return new WaitForSeconds(0.4f);
            
            while (true)
            {
                if (DistanceToGround(out var distance))
                {
                    if(distance <= _minDistanceToGround)
                        OnGrounded?.Invoke(true);
                }
                else
                {
                    OnGrounded?.Invoke(false);
                }

                yield return null;
            }
        }

        private bool DistanceToGround(out float distance)
        {
            var ray = new Ray(_ownerTransform.position, Vector3.down);
            if (Physics.Raycast(ray, out var hitInfo, 100.0f, _groundLayer))
            {
                distance = hitInfo.distance;
                return true;
            }

            distance = 0.0f;
            return false;
        }
    }
}