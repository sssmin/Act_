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
    public SO_Item.EItemCategory itemCategory;
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
    

    public void CreateRandomItem(SO_Item.EItemCategory itemCategory)
    {
        switch (itemCategory)
        {
            case SO_Item.EItemCategory.Weapon:
                CalcProbability(itemCategory,"WeaponMat", weaponProbabilityInfos);
                break;
            case SO_Item.EItemCategory.Armor:
                CalcProbability(itemCategory,"ArmorMat", armorProbabilityInfos);
                break;
            case SO_Item.EItemCategory.Acc:
                CalcProbability(itemCategory,"AccMat", accProbabilityInfos);
                break;
        }
    }

    private void CalcProbability(SO_Item.EItemCategory itemCategory, string equipmentMatId, List<ProbabilityInfo> probabilityInfos)
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
    
    private void Exchange(string createdItemId, string equipmentMatId, SO_Item.EItemCategory itemCategory)
    {
        SO_Item createdItem = null;
        
        if (itemCategory == SO_Item.EItemCategory.Weapon)
        {
            createdItem  = GI.Inst.ResourceManager.GetItemDataCopy(createdItemId);
            ((SO_BaseWeapon)createdItem).Element = (EWeaponElement)Random.Range(0, 4);
        }
        else
        {
            createdItem  = GI.Inst.ResourceManager.GetItemData(createdItemId);
        }
        
        RequireMatAmount requireMatAmount = requireMatAmounts.FirstOrDefault(r => r.itemCategory == itemCategory);
        
        SO_Item equipmentMat = GI.Inst.ResourceManager.GetItemData(equipmentMatId);
        GI.Inst.ListenerManager.SubItem(equipmentMat, false, requireMatAmount.requireEquipmentMatAmount);
        
        SO_Item sharedMat = GI.Inst.ResourceManager.GetItemData("SharedMat");
        GI.Inst.ListenerManager.SubItem(sharedMat, false, requireMatAmount.requireSharedMatAmount);
        
        GI.Inst.ListenerManager.AddItem(createdItem, true, 1);
        
        GI.Inst.UIManager.SetCraftResult(createdItem.itemName);
    }
    
}
