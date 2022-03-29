namespace Infrastructure.Services.SkinChanger
{
    public interface ISkinChanger
    {
        void InitSkinContainer();
        bool TrySetSkin(string skinName, ISkin skin);
        void SetDefaultMainActorSkin(ISkin skin);
        void SetDefaultEnemyActorSkin(ISkin skin);
        void SetSkin(int hash, ISkin skin);
    }
}