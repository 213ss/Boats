using Actors;
using Logic.Island;
using UnityEngine;

namespace Logic.Triggers
{
    public class PearseTrigger : MonoBehaviour
    {
        [SerializeField] private bool _isFinish;
        
        [Header("Maybe is null")]
        [SerializeField] private BaseIsland _baseIsland;

        private void Start()
        {
            if(_baseIsland == null)
                _baseIsland = GetComponentInParent<BaseIsland>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var actor = other.GetComponentInParent<Actor>();
            
            if (actor != null && _isFinish == false)
            {
                _baseIsland.DeliveryActor(actor);
            }
        }
    }
}
