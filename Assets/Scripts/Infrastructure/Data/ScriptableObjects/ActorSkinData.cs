using UnityEngine;

namespace Infrastructure.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Skins/Skin data", fileName = "Skin data")]
    public class ActorSkinData : ScriptableObject
    {
        public string SkinName => _skinName;
        public string SkinDescription => _skinDescription;
        public int SkinCost => _skinCost;
        public GameObject SkinPrefab => _skinPrefab;
        
        
        [SerializeField] private string _skinName;
        [SerializeField] private string _skinDescription;
        [SerializeField] private int _skinCost;
        [SerializeField] private GameObject _skinPrefab;
    }
}