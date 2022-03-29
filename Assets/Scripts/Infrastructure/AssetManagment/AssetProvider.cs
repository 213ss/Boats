using UnityEngine;
using Zenject;

namespace Infrastructure.AssetManagment
{
    public class AssetProvider : MonoBehaviour, IAssets
    {
        [Inject] private DiContainer _diContainer;
        
        public GameObject Instantiate(string filePath)
        {
            var spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            return SpawnFromDI(filePath, spawnPoint.transform.position);
        }

        public GameObject Instantiate(string filePath, Vector3 at)
        {
            return SpawnFromDI(filePath, at);
        }

        private GameObject SpawnFromDI(string path, Vector3 position)
        {
            return _diContainer.InstantiatePrefabResource(path, position, Quaternion.identity, null);
        }
    }
}