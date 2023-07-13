using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName ="Data/Item/Consumable")]
public class Consumable : Item
{
    public Consumable()
    {
        ItemCategory = EItemCategory.Consumable;
        amount = 1;
    }
    [Header("Consumable")]
    public EConsumableType consumableType;
    public float itemCooltime;

    [Header("DropAmount")] 
    public int dropChance;
    public int minDropAmount;
    public int maxDropAmount;
    
    [Header("Price")] 
    public int storeSellPrice; //상점에서의 가격

    
    public void ItemCopy(Consumable item)
    {
        itemId = item.itemId;
        ItemCategory = item.ItemCategory;
        itemName = item.itemName;
        itemDesc = item.itemDesc;
        itemIcon = item.itemIcon;
        //bIsCanStack = item.bIsCanStack;
        maxStackSize = item.maxStackSize;
        name = item.name;
        dropChance = item.dropChance;
        minDropAmount = item.minDropAmount;
        maxDropAmount = item.maxDropAmount;
    }

    public virtual void UseItem(StatManager castStatManager)
    {
        
    }

}