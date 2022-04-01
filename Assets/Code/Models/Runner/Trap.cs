using System;
using Runner;
using UnityEngine;

namespace Code.Models.Runner
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private int count = 100;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<CoinsCollector>(out var collector))
            {
                int tmpCount = count;
                if (collector.CollectedCoins <= tmpCount)
                {
                    tmpCount = collector.CollectedCoins;
                }

                collector.CollectedCoins -= tmpCount;
                collector.Buy(tmpCount,true,null);

                
                _animator.SetTrigger("collapse");
                Destroy(gameObject, 0.5f);
            }
        }
    }
}