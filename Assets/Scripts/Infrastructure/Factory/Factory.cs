using UnityEngine;

namespace Infrastructure.Factory
{
    public abstract class Factory : MonoBehaviour
    {
        public virtual GameObject CreateGameObject(GameObject prefab)
        {
            return null;
        }
    }
}