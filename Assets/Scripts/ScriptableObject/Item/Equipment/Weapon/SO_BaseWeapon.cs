using UnityEngine;

public enum EWeaponElement
{
    None,
    Fire,
    Water,
    Leaf,
}

[CreateAssetMenu(fileName = "Weapon_", menuName ="Data/Item/Weapon")]
public class SO_BaseWeapon : SO_Equipment
{
    public SO_BaseWeapon()
    {
        ItemCategory = EItemCategory.Weapon;
    }
    [Header("Equipment")]
    public EWeaponType weaponType;

    public EWeaponElement Element { get; set; } = EWeaponElement.None;
    public int EnhanceLevel { get; set; }
    

    public void InitEnhanceStat()
    {
        if (EnhanceLevel <= 0) return;
        float addToValue = 0;
        foreach (EnhanceValueByRarity enhanceValueByRarity in GI.Inst.ResourceManager.WeaponEnhanceValueByLevel.enhanceValueByRarities)
        {
            if (enhanceValueByRarity.rarity == rarity)
            {
                for (int i = 0; i < enhanceValueByRarity.enhanceValue.Count; i++)
                {
                    addToValue += enhanceValueByRarity.enhanceValue[i];
                    if (i == EnhanceLevel - 1)
                        break;
                }
                break;
            }
        }
       
        foreach (Stat stat in itemStats)
        {
            stat.ClearModifier();
            stat.AddModifier(addToValue);
        }
    }
    
}
