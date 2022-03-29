using System;
using Logic.Weapon.Weapons;
using UnityEngine;

namespace Logic.Triggers
{
    public class GoldTriggerDetector : MonoBehaviour
    {
        public event Action OnGoldTriggerEnter;
        public event Action OnGoldTriggerExit;

        [SerializeField] private ShovelWeapon _shovelWeapon;
        [SerializeField] private TriggerObserver _triggerObserver;

        private void Start()
        {
            _triggerObserver.TriggerEnter += GoldTriggerEnter;
            _triggerObserver.TriggerExit += GoldTriggerExit;
        }

        private void GoldTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent<GoldAreaTrigger>(out var goldTrigger))
            {
                _shovelWeapon.SetGoldTrigger(goldTrigger);
                OnGoldTriggerEnter?.Invoke();
            }
        }

        private void GoldTriggerExit(Collider collider)
        {
            if (collider.TryGetComponent<GoldAreaTrigger>(out var goldTrigger))
            {
                OnGoldTriggerEnter?.Invoke();
                _shovelWeapon.SetGoldTrigger(null);
            }
        }
    }
}