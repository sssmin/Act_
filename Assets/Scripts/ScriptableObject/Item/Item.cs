using System;
using UnityEngine;


public class Item : ScriptableObject, IComparable<Item>
{
    [Header("Info")] 
    [SerializeField] public string itemId;
    [SerializeField] public EItemCategory ItemCategory;
    [SerializeField] public string itemName;
    [SerializeField] public string itemDesc;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public bool bIsCanStack;

    [HideInInspector] public int amount;
    
    [SerializeField] public int maxStackSize = 50;
    
    public virtual void DataCopy(Item item)
    {
        itemId = item.itemId;
        ItemCategory = item.ItemCategory;
        itemName = item.itemName;
        itemDesc = item.itemDesc;
        itemIcon = item.itemIcon;
        bIsCanStack = item.bIsCanStack;
        maxStackSize = item.maxStackSize;
        
        name = item.name;
    }
    
    public enum EItemCategory
    {
        Weapon,
        Armor,
        Acc,
        Consumable,
        Etc,
    
        Max
    }

    public enum EWeaponType
    {
        None,
        Dagger,
        Axe,
        Bow,
    
        Max
    }

    public enum EArmorType
    {
        None,
        Helmet,
        Armor,
        Gauntlet,
        Boots,
    
        Max
    }

    public enum EAccType
    {
        None,
        Necklace,
        Ring,
    
        Max
    }

    public enum EConsumableType
    {
        HpPotion,
        AtkBoostPotion,
    }

    public enum EEtcCategory
    {
        Quest,
        SkillMat,
        EquipmentMat
        
    }

    public enum ERarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum EItemHotkeyOrder
    {
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Max
    }
    
    private void OnDestroy()
    {
        //Debug.Log($"{itemId} 아이템 파괴");
    }

    public int CompareTo(Item other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var itemIdComparison = string.Compare(itemId, other.itemId, StringComparison.Ordinal);
        if (itemIdComparison != 0) return itemIdComparison;
        var itemCategoryComparison = ItemCategory.CompareTo(other.ItemCategory);
        if (itemCategoryComparison != 0) return itemCategoryComparison;
        var itemNameComparison = string.Compare(itemName, other.itemName, StringComparison.Ordinal);
        if (itemNameComparison != 0) return itemNameComparison;
        var itemDescComparison = string.Compare(itemDesc, other.itemDesc, StringComparison.Ordinal);
        if (itemDescComparison != 0) return itemDescComparison;
        var bIsCanStackComparison = bIsCanStack.CompareTo(other.bIsCanStack);
        if (bIsCanStackComparison != 0) return bIsCanStackComparison;
        var amountComparison = amount.CompareTo(other.amount);
        if (amountComparison != 0) return amountComparison;
        return maxStackSize.CompareTo(other.maxStackSize);
    }
}
