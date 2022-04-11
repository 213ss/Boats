using System.Collections.Generic;
using Infrastructure.Services.Islands;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.IslandIndicator
{
    public class IslandIndicatorService : MonoBehaviour
    {
        [SerializeField] private Transform _parentIndicators;
        [SerializeField] private IndicatorSlot _indicatorPrefab;
        [SerializeField] private IndicatorSlot _indicatorLast;
        [Space]
        [SerializeField] private Sprite _completeSrpite;
        [SerializeField] private Sprite _nextSrpite;

        private IIslandService _islandService;

        private List<IndicatorSlot> _indicators = new List<IndicatorSlot>();


        [Inject]
        public void Construct(IIslandService islandService)
        {
            _islandService = islandService;
            _islandService.EventActorNewIsland += OnActorNewIsland;
        }

        private void OnActorNewIsland(int islandIndex)
        {
            UpdateWidgets(islandIndex, _islandService.GetCountIslands());
        }

        private void Start()
        {
            CreateWidgets(_islandService.GetCountIslands());
            UpdateWidgets(0, _islandService.GetCountIslands());
        }

        private void UpdateWidgets(int currentIsland, int countIsland)
        {
            for (int i = 0; i < _indicators.Count; ++i)
            {
                if (i <= currentIsland)
                {
                    _indicators[i].SetSprite(_completeSrpite);
                }
            }
            
            if((currentIsland + 1) < countIsland )
                _indicators[currentIsland + 1].SetSprite(_nextSrpite);
        }

        private void CreateWidgets(int count)
        {
            int currentInstance = 0;

            if (count == 1)
            {
                IndicatorSlot indicator = Instantiate(_indicatorLast, _parentIndicators);
                _indicators.Add(indicator);
                return;
            }

            while (currentInstance < count)
            {
                if ((currentInstance + 1) == count)
                {
                    IndicatorSlot indicator = Instantiate(_indicatorLast, _parentIndicators);
                    _indicators.Add(indicator);
                }
                else
                {
                    IndicatorSlot indicator = Instantiate(_indicatorPrefab, _parentIndicators);
                    _indicators.Add(indicator);
                }
                
                currentInstance++;
            }
        }
        
        
    }
}