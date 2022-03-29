using UnityEngine;

namespace Logic
{
    public interface IUIFollow
    {
        bool TryAddUIFollowObject(RectTransform who, Transform target, Vector2 offset);
        RectTransform DisableFollowAndReturn(Transform target);
    }
}