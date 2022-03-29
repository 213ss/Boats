using UnityEngine;

namespace Infrastructure.Services.Gold
{
    public interface IGoldIndicator
    {
        void EnableIndicator(Transform who);
        void DisableIndicator(Transform who);
    }
}