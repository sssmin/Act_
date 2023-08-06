using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName ="Data/Item/Consumable")]
public class SO_Consumable : SO_Item
{
    public SO_Consumable()
    {
        ItemCategory = EItemCategory.Consumable;
        amount = 1;
    }
    [Header("Consumable")]
    public EConsumableType consumableType;
    public float itemCooltime;
    
    
    [Header("Price")] 
    public int storeSellPrice; //상점에서의 가격

    
    public void ItemCopy(SO_Consumable item)
    {
        itemId = item.itemId;
        ItemCategory = item.ItemCategory;
        itemName = item.itemName;
        itemDesc = item.itemDesc;
        itemIcon = item.itemIcon;
        maxStackSize = item.maxStackSize;
        name = item.name;
    }

    public virtual void UseItem(StatManager castStatManager)
    {
    }

}
