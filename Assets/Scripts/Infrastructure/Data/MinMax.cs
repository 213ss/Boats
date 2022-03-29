using UnityEngine;

namespace Infrastructure.Data
{
    public struct MinMax
    {
        public float Max;
        public float Min;

        public MinMax(float min, float max)
        {
            Max = max;
            Min = min;
        }

        public float GetRandomRange()
        {
            return Random.Range(Min, Max);
        }
    }
}