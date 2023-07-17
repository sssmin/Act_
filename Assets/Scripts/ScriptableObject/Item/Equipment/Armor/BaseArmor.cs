using UnityEngine;

[CreateAssetMenu(fileName = "Armor_", menuName ="Data/Item/Armor")]
public class BaseArmor : Equipment
{
    public BaseArmor()
    {
        ItemCategory = EItemCategory.Armor;
    }
    
    public EArmorType armorType;
    
}