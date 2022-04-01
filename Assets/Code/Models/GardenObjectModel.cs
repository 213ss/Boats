using System;

namespace Code.Models
{
    [Serializable]
    public class GardenObjectModel
    {
        public int Cost { get; set; }
        public GurdenObjectEnum Type { get; set; }
        public bool IsCollected { get; set; }

        public bool IsNew { get; set; }
    }
}