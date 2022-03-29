using UnityEngine;

namespace Infrastructure.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Tools/Weapons/Weapon data", fileName = "Weapon data")]
    public class WeaponData : ScriptableObject
    {
        public string WeaponName => _weaponName;
        public GameObject WeaponPrefab => _weaponPrefab;
        
        [SerializeField] private string _weaponName;
        [SerializeField] private GameObject _weaponPrefab;
        
        
    }
}