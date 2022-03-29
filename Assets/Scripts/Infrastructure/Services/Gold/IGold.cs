using System;

namespace Scripts.Infrastructure.Services.Gold
{
    public interface IGold
    {
        event Action<float> OnGoldChange;
        float CurrentCount { get; }
    }

    public interface IGoldChanger : IGold
    {
        void AddGold(float amount);
        void AddGoldDelay(float goldCount, int countIteration, float intervalSeconds);

        void SubstractionGold(float count);
    }
}