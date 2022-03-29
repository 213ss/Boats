using Actors;
using UnityEngine;

namespace Enemy
{
    public interface IAIMovement : IMovement
    {
        void SetDestination(Vector3 point);
        float GetRemainingDistance();
        void Repath();
        bool IsAFK();
    }
}