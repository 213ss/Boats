using UnityEngine;

namespace Logic.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        public string WeaponName => _weaponName;
        
        [SerializeField] protected string _weaponName;

        public virtual void UseWeapons()
        {
            
        }

        public virtual void WeaponOnMouseUp()
        {
            
        }
        
        
    }
}