using System;
using Logic.Triggers;

namespace Logic.Weapon.Weapons
{
    public interface IShovelWeapon : IWeapon
    {
        event Action OnShovelStrike;
        event Action OnExcavated;
        bool IsStayInGoldArea { get; set; }
        bool IsTriggerStay { get; set; }
        void SetGoldExcavation(float goldPrize);
        void SetGoldTrigger(GoldAreaTrigger trigger);
        void Excavate();
    }

    public interface IWeapon
    {
        void UseWeapons();
        void WeaponOnMouseUp();
    }
}