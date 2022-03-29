using UnityEngine;

namespace Actors
{
    public interface IMovement
    {
        bool IsMovement { get; }
        void Move(Vector3 direction);
        void SetMoveSpeed(float speed);
        void EnableMoveComponent();
        void DisableMoveComponent();
        bool IsStopped { get; }
        bool IsDisableMovement { get; }
    }
}