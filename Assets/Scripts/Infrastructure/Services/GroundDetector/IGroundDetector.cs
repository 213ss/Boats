using System;

namespace Infrastructure.Services.GroundDetector
{
    public interface IGroundDetector
    {
        event Action<bool> OnGrounded;
        void EnableGroundChecker();
        void DisableGroundChecker();
    }
}