using UnityEngine;

namespace Infrastructure.Services.InputServices
{
    public class InputServices : MonoBehaviour, IInputServices
    {
        [SerializeField] private Joystick _joystick;
        
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        
        public Vector3 Axes()
        {
            return new Vector3(_joystick.Horizontal, 0.0f, _joystick.Vertical);
            //return new Vector3(Input.GetAxis(HorizontalAxis), 0.0f, Input.GetAxis(VerticalAxis));
            //return new Vector3(_joystick.Horizontal, 0.0f, _joystick.Vertical);
        }

        public bool IsLeftMouseButton()
        {
            return Input.GetMouseButton(0);
        }

        public bool IsRightMouseButtonDown()
        {
            return Input.GetMouseButtonDown(1);
        }

        public bool IsLeftMouseButtonUp()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}