using Infrastructure.Services.UIDirect;
using UnityEngine;

namespace Infrastructure.Services.BoatsIndicator
{
    public class BoatsIndicatorService : MonoBehaviour, IBoatsIndicatorService
    {
        [SerializeField] private UIDirectToWorldObject[] _indicators;

        public void SetBoatsIndicatorObject(Transform[] boatsTransforms, int count)
        {
            for (int i = 0; i < _indicators.Length; ++i)
            {
                if (i < count)
                {
                    _indicators[i].SetFollowingObject(boatsTransforms[i]);
                    _indicators[i].EnableIndicator();
                }
                else
                {
                    _indicators[i].DisableIndicator();
                }
            }
        }

        public void DisableIndicators()
        {
            for (int i = 0; i < _indicators.Length; ++i)
            {
                _indicators[i].DisableIndicator();
            }
        }
    }
}