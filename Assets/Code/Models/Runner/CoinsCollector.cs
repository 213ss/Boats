using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Models.Runner;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Runner
{
    public class CoinsCollector : MonoBehaviour
    {
        [SerializeField] private Transform coinPlace;
        [SerializeField] private Transform coinfolowTarget;
        [SerializeField] private TMP_Text coinsCountPlace;
        [SerializeField] private int coinCost = 100;
        [SerializeField] private Animator bagAnimController;
        [SerializeField] private float timeToResetDelta = 1f;
        public int CoinCost => coinCost;

        private Stack<Transform> coins;
        public int CollectedCoins { get; set; }

        private int tmpPosCoin = 0;
        private int tmpNegCoin = 0;
        
        private void Awake()
        {
            coins = new Stack<Transform>();
        }

        public Transform GetCoinPlace(Transform coin)
        {
            coinPlace.position += Vector3.up * coinPlace.localScale.y;

            var cn = coin.GetComponent<Coin>();

            if (CollectedCoins == 0)
            {
                cn.target = coinfolowTarget;
            }
            else
            {
                cn.target = coins.First();
            }

            cn.isCollected = true;
            CollectedCoins += coinCost;
            coinsCountPlace.text = CollectedCoins.ToString();
            coins.Push(coin);
            return coinPlace;
        }

        public void Buy(int count, bool isPhysics, Transform target)
        {
            StartCoroutine(ThrowCoins(count / 100,isPhysics, target));
        }

        private IEnumerator ThrowCoins(int count, bool isPhysics, Transform target)
        {
            for (int i = 0; i < count; i++)
            {
                coinsCountPlace.text = CollectedCoins.ToString();

                var coin = coins.Pop();

                var cn = coin.GetComponent<Coin>();
                cn.isCollected = false;
                coinPlace.position -= Vector3.up * coinPlace.localScale.y;
                coin.SetParent(null);
                if (isPhysics)
                {
                    var coinrb = coin.gameObject.AddComponent<Rigidbody>();
                    coinrb.AddForce(Vector3.up * 5f);
                }
                else
                {
                    coin.DOMove(target.position, 0.2f);
                    coin.DOScale(Vector3.zero, 0.2f);
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        public void NoMoneyPing()
        {
            bagAnimController.SetTrigger("ping");
        }
    }
}