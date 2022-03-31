using Infrastructure.Data.ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Services.SkinChanger
{
    public class Skin : MonoBehaviour, ISkin
    {
        public ActorSkinData GetSkinData => _actorSkinData;
        
        [SerializeField] private Transform _staticMeshSocket;

        private ActorSkinData _actorSkinData;
        private GameObject _skinGameObject;


        public void ChangeSkin(ActorSkinData skinData)
        {
            if (_skinGameObject != null)
            {
                Destroy(_skinGameObject);
            }

            _actorSkinData = skinData;
            _skinGameObject = Instantiate(skinData.SkinPrefab, _staticMeshSocket);
        }

        public SkinnedMeshRenderer GetMeshRenderer()
        {
            return _staticMeshSocket.GetComponentInChildren<SkinnedMeshRenderer>();
        }
    }
}