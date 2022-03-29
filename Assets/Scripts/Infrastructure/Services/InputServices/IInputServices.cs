using UnityEngine;

namespace Infrastructure.Services.InputServices
{
    public interface IInputServices
    {
        Vector3 Axes();
        bool IsLeftMouseButton();
        bool IsLeftMouseButtonUp();
        bool IsRightMouseButtonDown();
    }
}
