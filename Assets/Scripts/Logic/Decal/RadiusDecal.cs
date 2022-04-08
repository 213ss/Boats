using System;
using Logic.Weapon.Weapons;
using UnityEngine;

namespace Logic.Decal
{
    public class RadiusDecal : MonoBehaviour
    {
        [SerializeField] private Transform _decalTransform;
        [SerializeField] private ShovelWeapon _shovelWeapon;
        
        
        private float _decalRadius;


        private void Start()
        {
            if (_decalTransform == null)
                _decalTransform = GetComponent<Transform>();
            
            SetDecalRadius(_shovelWeapon.GetHolDistance());
        }

        public void HideDecal()
        {
            gameObject.SetActive(false);
        }

        public void SetDecalRadius(float radius)
        {
            _decalRadius = (radius - 1.2f) * 2.0f;
            RecalculateRadius();
        }

        private void RecalculateRadius()
        {
            _decalTransform.localScale = new Vector3(_decalRadius, _decalRadius, _decalRadius);
        }
        
    }
}
