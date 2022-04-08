using UnityEngine;

namespace Infrastructure.Services.BoatsIndicator
{
    public interface IBoatsIndicatorService
    {
        void SetBoatsIndicatorObject(Transform[] boatsTransforms, int count);
        void DisableIndicators();
    }
}