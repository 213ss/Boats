using System.Collections.Generic;
using Actors;
using Infrastructure.Services.Gold;
using Logic.Weapon.Weapons;
using Scripts.Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace Logic.Triggers
{
    public class GoldAreaTrigger : MonoBehaviour
    {
        public Transform ThisTransform { get; private set; }
        public bool IsCooldown => _isCooldown;
        
        [SerializeField] private float _goldPrize;

        private IGoldIndicator _goldIndicator;
        private bool _isCooldown;

        private List<Actor> _insideActors = new List<Actor>();


        [Inject]
        public void Construct(IGoldIndicator goldIndicator)
        {
            _goldIndicator = goldIndicator;
        }

        private void Start()
        {
            ThisTransform = GetComponent<Transform>();
        }

        public void SetGoldPrize(float goldPrize)
        {
            _goldPrize = goldPrize;
        }

        private void OnTriggerEnter(Collider other)
        {
            var actor = other.GetComponentInParent<Actor>();
            if (actor != null && _isCooldown == false)
            {
                _insideActors.Add(actor);
                IShovelWeapon shovel = actor.GetComponent<IShovelWeapon>();

                shovel.SetGoldExcavation(_goldPrize);
                shovel.SetGoldTrigger(this);
                shovel.IsStayInGoldArea = true;

                if(actor.ActorTeam == Team.Player_0)
                    _goldIndicator.EnableIndicator(actor.ActorTransform);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_isCooldown)
            {
                foreach (var actor in _insideActors)
                {
                    actor.GetComponent<ShovelWeapon>().IsStayInGoldArea = false;

                    if(actor.ActorTeam == Team.Player_0)
                        _goldIndicator.DisableIndicator(actor.ActorTransform);
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var actor = other.GetComponentInParent<Actor>();
            if (actor != null)
            {
                _insideActors.Remove(actor);
                actor.GetComponent<IShovelWeapon>().IsTriggerStay = false;
                
                if (_isCooldown == false)
                {
                    IShovelWeapon shovel = actor.GetComponent<IShovelWeapon>();

                    shovel.IsStayInGoldArea = false;
                    shovel.SetGoldTrigger(null);
                }

                if(actor.ActorTeam == Team.Player_0)
                    _goldIndicator.DisableIndicator(actor.ActorTransform);
                
            }
        }

        public void CloseTrigger()
        {
            _isCooldown = true;
        }
        
#if UNITY_EDITOR

        [Header("Only editor mode")]
        public Color _colorWire;

        public float _rayLength;

        public bool IsDisplaing = true;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = _colorWire;
            Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.extents * 2.0f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up *_rayLength);
        }

#endif

    }
}