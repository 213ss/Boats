using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Utils
{
    public static class RandomExt
    {
        [CanBeNull]
        public static T ChooseOne<T>(this IEnumerable<T> values)
        {
            var castedValues = values.ToArray();
            if (castedValues.Length <= 0)
                return default;
            var randomIndex = Random.Range(0, castedValues.Length);
            return castedValues[randomIndex];
        }
    }
}