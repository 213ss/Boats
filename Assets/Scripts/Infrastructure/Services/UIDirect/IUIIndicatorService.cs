using UnityEngine;

namespace Infrastructure.Services.UIDirect
{
    public interface IUIIndicatorService
    {
        void SetFollowingObject(Transform origin);
        void EnableIndicator();
        void DisableIndicator();
    }
}