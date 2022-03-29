using UnityEngine;

namespace Scripts.Infrastructure.Services.Camera
{
    public interface ICameraFollow
    {
        void SetFollow(Transform followTransform);
    }
}