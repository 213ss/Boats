using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.VFXGoldBurst
{
    public class VFXGoldBurst : MonoBehaviour
    {
        [SerializeField] private Transform _goldPrefab;
        [SerializeField] private float _intervalSpawn;
        [SerializeField] private float _intervalDestination;

        private Transform _originTransform;

        private List<Transform> _coinObjects;
        
        public void RunGoldVFX(Transform origin, int countObjects)
        {
            
        }

        private void FillCoinsArray()
        {
            if (_coinObjects != null) return;
            
            
        }
    }
}