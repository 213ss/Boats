using System;
using System.Collections;
using Infrastructure.Data;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;

namespace Infrastructure.Services.Gold
{
    public class Gold : MonoBehaviour, IGoldChanger, ISavedProgress
    {
        public event Action<float> OnGoldChange;
        
        public float CurrentCount => _currentCount;

        [SerializeField] private ParticleSystem _glowVFX;
        
        private float _currentCount = 0;


        public void AddGold(float amount)
        {
            if(amount <= 0) return;

            _currentCount += amount;
            OnGoldChange?.Invoke(_currentCount);
        }

        public void AddGoldDelay(float goldCount, int countIteration, float intervalSeconds)
        {
            StartCoroutine(ChangeDelayProcess(goldCount, countIteration, intervalSeconds));
        }

        public void SubstractionGold(float count)
        {
            _currentCount -= count;
            
            if (_currentCount < 0.0f) _currentCount = 0.0f;
            
            OnGoldChange?.Invoke(_currentCount);
        }

        private IEnumerator ChangeDelayProcess(float goldCount, int countIteration, float intervalSeconds)
        {
            float part = goldCount / countIteration;

            for (int i = 0; i < countIteration; i++)
            {
                if(_glowVFX.isPlaying == false)
                    _glowVFX.Play();
                
                AddGold(part);
                yield return new WaitForSeconds(intervalSeconds);
            }
            
            
        }

        public void LoadProgress(PlayerProgress progress)
        {
            
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            
        }
    }
}