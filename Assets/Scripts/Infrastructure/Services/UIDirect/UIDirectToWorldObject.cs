using UnityEngine;

namespace Infrastructure.Services.UIDirect
{
    public class UIDirectToWorldObject : MonoBehaviour, IUIIndicatorService
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Transform followingTransform;
        [SerializeField] private Vector2 offset;

        [SerializeField] private RectTransform parentRT;
        [SerializeField] private RectTransform _whoFollowing;
        [SerializeField] private RectTransform _canvasRectTransform;

        private Vector2 canvasSize;

        private bool _isEnable = true;

        private void Start()
        {
            canvasSize = _canvasRectTransform.sizeDelta;
        }

        public void SetFollowingObject(Transform origin)
        {
            followingTransform = origin;
        }

        public void EnableIndicator()
        {
            _isEnable = true;
            _whoFollowing.gameObject.SetActive(true);
            LateUpdate();
        }

        private void LateUpdate()
        {
            if(_isEnable == false) return;
            
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint( _camera, followingTransform.position );
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle( parentRT, screenPoint, _uiCamera, 
                out Vector2 anchoredPos);
            
            Vector2 positionOffset = anchoredPos + offset;
            Vector2 pos1 = positionOffset;

            positionOffset.x = Mathf.Max( -canvasSize.x / 2f, positionOffset.x );
            positionOffset.x = Mathf.Min( canvasSize.x / 2f, positionOffset.x );
            positionOffset.y = Mathf.Max( -canvasSize.y / 2f, positionOffset.y );
            positionOffset.y = Mathf.Min( canvasSize.y / 2f, positionOffset.y );

            _whoFollowing.anchoredPosition = positionOffset;
            
            float angle = Mathf.Atan2( pos1.y, pos1.x ) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.identity;
            q.eulerAngles = new Vector3( 0, 0, angle );
            _whoFollowing.localRotation = q;
        }

        public void DisableIndicator()
        {
            _isEnable = false;
            _whoFollowing.gameObject.SetActive(false);
        }
    }
}