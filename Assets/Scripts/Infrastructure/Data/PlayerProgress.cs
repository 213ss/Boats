using System;

namespace Infrastructure.Data
{
    [Serializable]
    public struct WorldData
    {
        public Vector3Data MainActorPosition;
    }
    
    [Serializable]
    public class PlayerProgress
    {
        public float Gold;
        public string SkinName;
        public WorldData WorldData { get; private set; }
    }
}
