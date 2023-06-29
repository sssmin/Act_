using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_", menuName ="Data/Item/Weapon")]
public class BaseWeapon : Equipment
{
    public BaseWeapon()
    {
        ItemCategory = EItemCategory.Weapon;
    }
    [Header("Equipment")]
    public EWeaponType weaponType;

    public override void DataCopy(Item item)
    {
        base.DataCopy(item);
        
        weaponType = ((BaseWeapon)item).weaponType;
    }

    public override void Init(StatManager ownerStatManager)
    {
        base.Init(ownerStatManager);
    }
}
