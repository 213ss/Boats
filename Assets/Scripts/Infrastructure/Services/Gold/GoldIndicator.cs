using System.Collections.Generic;
using Logic;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Gold
{
    public class GoldIndicator : MonoBehaviour, IGoldIndicator
    {
        [SerializeField] private Vector2 _indicatorOffset;
        [SerializeField] private RectTransform _parentObject;
        [SerializeField] private RectTransform _indicatorPrefab;

        [Header("Pool settings")] 
        [SerializeField] private int _countObjects;

        private Queue<RectTransform> _poolQueue = new Queue<RectTransform>();

        private IUIFollow _uiFollow;
        

        [Inject]
        private void Construct(IUIFollow uiFollow)
        {
            _uiFollow = uiFollow;
            
            for (int i = 0; i < _countObjects; ++i)
            {
                AddObjectToPool();
            }
        }
        

        public void EnableIndicator(Transform who)
        {
            var rectIndicator = GetIndicator();
            var successAdd = _uiFollow.TryAddUIFollowObject(rectIndicator, who, _indicatorOffset);
            
            if(successAdd == false)
                ReturnIndicator(rectIndicator);
        }

        public void DisableIndicator(Transform who)
        {
            if(who == null) return;
            
            RectTransform indicator = _uiFollow.DisableFollowAndReturn(who);
            
            if(indicator != null)
                ReturnIndicator(indicator);
        }
        

        private RectTransform GetIndicator()
        {
            if (_poolQueue.Count == 0)
            {
                AddObjectToPool();
            }

            var indicator = _poolQueue.Dequeue();
            indicator.gameObject.SetActive(true);

            return indicator;
        }

        private void AddObjectToPool()
        {
            var indicator = Instantiate(_indicatorPrefab, _parentObject);
            indicator.gameObject.SetActive(false);
            ReturnIndicator(indicator);
        }

        private void ReturnIndicator(RectTransform poolObject)
        {
            poolObject.gameObject.SetActive(false);
            _poolQueue.Enqueue(poolObject);
        }
    }
}