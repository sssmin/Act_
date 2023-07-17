using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class ProbabilityInfo
{
    public string itemId;
    public float probability;
}

[Serializable]
public class RequireMatAmount
{
    public Item.EItemCategory itemCategory;
    public int requireEquipmentMatAmount;
    public int requireSharedMatAmount;
}

[CreateAssetMenu(fileName = "ItemCraft", menuName ="Data/ItemCraft")]
public class ItemCraft : ScriptableObject
{
    public List<ProbabilityInfo> weaponProbabilityInfos = new List<ProbabilityInfo>();
    public List<ProbabilityInfo> armorProbabilityInfos = new List<ProbabilityInfo>();
    public List<ProbabilityInfo> accProbabilityInfos = new List<ProbabilityInfo>();
    public Sprite weaponMatIcon;
    public Sprite armorMatIcon;
    public Sprite accMatIcon;
    public List<RequireMatAmount> requireMatAmounts = new List<RequireMatAmount>();
    

    public void CreateRandomItem(Item.EItemCategory itemCategory)
    {
        switch (itemCategory)
        {
            case Item.EItemCategory.Weapon:
                CalcProbability(itemCategory,"WeaponMat", weaponProbabilityInfos);
                break;
            case Item.EItemCategory.Armor:
                CalcProbability(itemCategory,"ArmorMat", armorProbabilityInfos);
                break;
            case Item.EItemCategory.Acc:
                CalcProbability(itemCategory,"AccMat", accProbabilityInfos);
                break;
        }
    }

    private void CalcProbability(Item.EItemCategory itemCategory, string equipmentMatId, List<ProbabilityInfo> probabilityInfos)
    {
        for (int i = 0; i < probabilityInfos.Count + 1; ++i)
        {
            float randValue = Random.Range(0f, 100f);
            float addValue = 0f;
            for (int j = 0; j < probabilityInfos.Count; ++j)
            {
                addValue += probabilityInfos[j].probability;
                if (randValue <= addValue)
                {
                    Exchange(probabilityInfos[j].itemId, equipmentMatId, itemCategory);
                    return;
                }
            }
        }
    }
    
    private void Exchange(string createdItemId, string equipmentMatId, Item.EItemCategory itemCategory)
    {
        Item createdItem = null;
        
        if (itemCategory == Item.EItemCategory.Weapon)
        {
            createdItem  = GI.Inst.ResourceManager.GetItemDataCopy(createdItemId);
            ((BaseWeapon)createdItem).Element = (EWeaponElement)Random.Range(0, 4);
        }
        else
        {
            createdItem  = GI.Inst.ResourceManager.GetItemData(createdItemId);
        }
        
        RequireMatAmount requireMatAmount = requireMatAmounts.FirstOrDefault(r => r.itemCategory == itemCategory);
        
        Item equipmentMat = GI.Inst.ResourceManager.GetItemData(equipmentMatId);
        GI.Inst.ListenerManager.SubItem(equipmentMat, false, requireMatAmount.requireEquipmentMatAmount);
        
        Item sharedMat = GI.Inst.ResourceManager.GetItemData("SharedMat");
        GI.Inst.ListenerManager.SubItem(sharedMat, false, requireMatAmount.requireSharedMatAmount);
        
        GI.Inst.ListenerManager.AddItem(createdItem, true, 1);
        
        GI.Inst.UIManager.SetCraftResult(createdItem.itemName);
    }
    
}
