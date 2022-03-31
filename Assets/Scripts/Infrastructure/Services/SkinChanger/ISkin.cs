using Infrastructure.Data.ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Services.SkinChanger
{
    public interface ISkin
    {
        public ActorSkinData GetSkinData { get; }
        public void ChangeSkin(ActorSkinData skinData);
        SkinnedMeshRenderer GetMeshRenderer();
    }
}