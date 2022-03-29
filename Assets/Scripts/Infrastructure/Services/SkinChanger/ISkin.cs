using Infrastructure.Data.ScriptableObjects;

namespace Infrastructure.Services.SkinChanger
{
    public interface ISkin
    {
        public ActorSkinData GetSkinData { get; }
        public void ChangeSkin(ActorSkinData skinData);
    }
}