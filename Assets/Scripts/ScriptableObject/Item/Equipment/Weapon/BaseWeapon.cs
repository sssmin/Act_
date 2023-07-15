using UnityEngine;

public enum EWeaponElement
{
    None,
    Fire,
    Water,
    Leaf,
}

[CreateAssetMenu(fileName = "Weapon_", menuName ="Data/Item/Weapon")]
public class BaseWeapon : Equipment
{
    public BaseWeapon()
    {
        ItemCategory = EItemCategory.Weapon;
    }
    [Header("Equipment")]
    public EWeaponType weaponType;

    public EWeaponElement Element { get; set; } = EWeaponElement.None;
    public int EnhanceLevel { get; set; } = 0; 

    protected override void DataCopy(Item item)
    {
        base.DataCopy(item);
        
        weaponType = ((BaseWeapon)item).weaponType;
    }

    public override void Init(StatManager ownerStatManager)
    {
        base.Init(ownerStatManager);
    }
}
