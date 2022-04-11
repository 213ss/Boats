using UnityEngine;

namespace Logic.UI
{
    public class Boats_follow_icone : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetFollow;
        [SerializeField] private RectTransform _currentTransform;


        private void Update()
        {
            if (_targetFollow.hasChanged)
            {
                Quaternion qt = _targetFollow.localRotation;
                Quaternion at2 = new Quaternion(-qt.x, -qt.y, -qt.z, qt.w);
                _currentTransform.localRotation = at2;
            }
        }
    }
}
