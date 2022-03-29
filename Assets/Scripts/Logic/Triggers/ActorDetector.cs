using System;
using Actors;
using UnityEngine;

namespace Logic.Triggers
{
    [RequireComponent(typeof(Actor))]
    public class ActorDetector : MonoBehaviour, IActorDetector
    {
        public event Action<Actor> OnDetectActor;
        public event Action OnDetect;

        [SerializeField] private float _distanceDetect;
        [SerializeField] private LayerMask _selectingLayers;

        private Transform _actorOwnerTransform;

        private bool _isDetecting;
        private Actor _ownerActor;

        private void Start()
        {
            _ownerActor = GetComponent<Actor>();
            _actorOwnerTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (_isDetecting)
            {
                Ray ray = new Ray(_actorOwnerTransform.position, _actorOwnerTransform.forward);
                
                if (Physics.Raycast(ray, out var hitResult, _distanceDetect, _selectingLayers))
                {
                    if (hitResult.collider.TryGetComponent<Actor>(out var actorDetect))
                    {
                        if (actorDetect.ActorTeam != _ownerActor.ActorTeam)
                        {
                            OnDetectActor?.Invoke(actorDetect);
                            return;
                        }
                    }
                    
                    OnDetect?.Invoke();
                }
            }
        }

        public void EnableDetect()
        {
            _isDetecting = true;
        }

        public void DisableDetect()
        {
            _isDetecting = false;
        }

#if UNITY_EDITOR

        [Header("Only editor")]
        public Color ColorHitBox;

        public bool IsDisplaying = true;
        
        private void OnDrawGizmos()
        {
            if (IsDisplaying)
            {
                Gizmos.color = ColorHitBox;
                Vector3 position = transform.position + transform.forward * _distanceDetect;
                Gizmos.DrawLine(transform.position, position);
            }
        }
#endif
        
    }
}
