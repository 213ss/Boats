using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public struct FollowObjectData
    {
        public RectTransform Following;
        public Vector2 Offset;

        public FollowObjectData(RectTransform following, Vector2 offset)
        {
            Following = following;
            Offset = offset;
        }
    }

    public class UIFollow : MonoBehaviour, IUIFollow
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private RectTransform _containerUI;

        private Dictionary<Transform, FollowObjectData> _followObjects = new Dictionary<Transform, FollowObjectData>();


        private void Start()
        {
            if(_uiCamera == null)
                _uiCamera = Camera.main;
            
        }

        public bool TryAddUIFollowObject(RectTransform who, Transform target, Vector2 offset)
        {
            if(_followObjects.ContainsKey(target)) return false;

            var followData = new FollowObjectData(who, offset);
            
            _followObjects.Add(target, followData);
            return true;
        }

        public RectTransform DisableFollowAndReturn(Transform target)
        {
            if (_followObjects.TryGetValue(target, out var following))
            {
                _followObjects.Remove(target);
                return following.Following;
            }
            
            return null;
        }

        private void LateUpdate()
        {
            if (_followObjects.Count > 0)
            {
                foreach (var follow in _followObjects)
                {
                    if (follow.Key == null || follow.Value.Following == null)
                    {
                        _followObjects.Remove(follow.Key);
                        continue;
                    }
                    
                    Vector2 positionOnScreen = RectTransformUtility.WorldToScreenPoint(_camera, follow.Key.position);

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_containerUI, positionOnScreen, _uiCamera,
                        out Vector2 anchoredPosition);

                    follow.Value.Following.anchoredPosition = anchoredPosition + follow.Value.Offset;
                }
            }
        }
    }
}
