using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Runner
{
    public class CoinsStack : MonoBehaviour
    {
        [SerializeField] private List<Transform> coins;
        [SerializeField] private float coinWayTime = 0.2f;
        [SerializeField] private float getCoinsDelay = 0.1f;
        [SerializeField] private float scaleModifier = 0.5f;
        [SerializeField] private GameObject effect;
        private bool _isCollected = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<CoinsCollector>(out var collector))
            {
                if(_isCollected)
                    return;

                effect.SetActive(false);
                _isCollected = true;

                StartCoroutine(GetCoins(collector));
            }
        }

        private IEnumerator GetCoins(CoinsCollector collector)
        {
            foreach (var coin in coins)
            {
                var place = collector.GetCoinPlace(coin);
                coin.SetParent(collector.transform);
                coin.DOLocalMove(place.localPosition, coinWayTime);
                coin.DORotate(Vector3.up * 90, coinWayTime);
                coin.DOScale(coin.transform.localScale * scaleModifier, coinWayTime);
                
                yield return  new WaitForSeconds(getCoinsDelay);
            }
        }
    }
}