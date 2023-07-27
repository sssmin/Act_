using UnityEngine;

[CreateAssetMenu(fileName = "Armor_", menuName ="Data/Item/Armor")]
public class SO_BaseArmor : SO_Equipment
{
    public SO_BaseArmor()
    {
        ItemCategory = EItemCategory.Armor;
    }
    
    public EArmorType armorType;
    
}