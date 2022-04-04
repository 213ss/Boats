using System;
using System.Collections;
using System.Collections.Generic;
using Actors;
using Logic.Coins;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.Services.VFXGoldBurst
{
    public class VFXGoldBurst : MonoBehaviour
    {
        public event Action OnMoveToTargetEnd;
        
        [SerializeField] private float _delay;
        [SerializeField] private float _lifeTime;
        [SerializeField] private iTween.EaseType _easeType;
        [SerializeField] private Vector3 _offset;

        [SerializeField] private CoinVFX _coinVFXPrefab;


        private List<CoinVFX> _coins = new List<CoinVFX>();

        private Actor _target;

        public void PlayGoldVFX(Vector3 spawnPosition, int countGoldObjects, Actor target)
        {
            _target = target;
            Vector3 position = spawnPosition;
            position.y = 0.0f;
            
            FillCoinsArray(position, countGoldObjects);
            AddForce();

            StartCoroutine(MoveToTarget());
        }

        private IEnumerator MoveToTarget()
        {
            float delay = _delay;
            while (delay >= 0.0f)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            
            
            for (int i = 0; i < _coins.Count; ++i)
            {
                _coins[i].DisablePhysics();
                
                iTween.MoveTo(_coins[i].gameObject, iTween.Hash("position", 
                    _target.ActorTransform.position + _offset, 
                    "easetype", _easeType, 
                    "ignoretimescale", true, "time", _lifeTime));
            }

            for (int i = 0; i < _coins.Count; i++)
            {
                Destroy(_coins[i].gameObject, _lifeTime);
            }

            _coins.Clear();
            OnMoveToTargetEnd?.Invoke();
        }


        private void FillCoinsArray(Vector3 position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                float randomX = GetRandomNumber(-0.4f, 0.4f);
                float randomZ = GetRandomNumber(-0.4f, 0.4f);

                Vector3 offsetPosition = new Vector3(position.x + randomX, position.y, position.z + randomZ);
                
                CoinVFX coinVFX = Instantiate(_coinVFXPrefab, offsetPosition, Quaternion.identity);
                _coins.Add(coinVFX);
            }
        }

        private void AddForce()
        {
            for (int i = 0; i < _coins.Count; i++)
            {
                float randomX = GetRandomNumber(-1.4f, 1.4f);
                float randomZ = GetRandomNumber(-1.4f, 1.4f);

                Vector3 offsetPosition = new Vector3(randomX, 5.0f, randomZ);
                
                Vector3 force = Vector3.up + offsetPosition;
                force *= 2.0f;
                
                _coins[i].EnablePhysics();
                _coins[i].AddForce(force);
            }
        }

        private float GetRandomNumber(float min, float max)
        {
            return Random.Range(min, max);
        }
    }
}