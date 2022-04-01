namespace DungeonMaster.Code.Storage
{
    public abstract class StorageObject
    {
        public abstract void LoadDefaults();
        public abstract void OnLoaded();
        public abstract void OnSaved();
    }
}