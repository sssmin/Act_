using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UI_Inven_EquippedSlotParent : MonoBehaviour
{
    public UI_Inven_EquippedSlot equippedWeaponSlot;

    public Dictionary<SO_Item.EArmorType, UI_Inven_EquippedSlot> equippedArmorSlots =
        new Dictionary<SO_Item.EArmorType, UI_Inven_EquippedSlot>();

    public Dictionary<SO_Item.EAccType, UI_Inven_EquippedSlot> equippedAccSlots =
        new Dictionary<SO_Item.EAccType, UI_Inven_EquippedSlot>();

    public void InitOnce()
    {
        InitArmorSlot(SO_Item.EArmorType.Helmet);
        InitAccSlot(SO_Item.EAccType.Necklace);
        InitArmorSlot(SO_Item.EArmorType.Armor);
        InitAccSlot(SO_Item.EAccType.Ring);
        InitArmorSlot(SO_Item.EArmorType.Gauntlet);
        InitWeaponSlot();
        InitArmorSlot(SO_Item.EArmorType.Boots);
    }

    private void InitWeaponSlot()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_EquippedSlot", transform);
        UI_Inven_EquippedSlot equippedSlotUI = go.GetComponent<UI_Inven_EquippedSlot>();
        equippedSlotUI.InitOnce(SO_Item.EItemCategory.Weapon, SO_Item.EArmorType.None, SO_Item.EAccType.None);
        equippedWeaponSlot = equippedSlotUI;
    }

    private void InitArmorSlot(SO_Item.EArmorType type)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_EquippedSlot", transform);
        UI_Inven_EquippedSlot equippedSlotUI = go.GetComponent<UI_Inven_EquippedSlot>();
        equippedSlotUI.InitOnce(SO_Item.EItemCategory.Armor, type, SO_Item.EAccType.None);
        equippedArmorSlots.Add(type, equippedSlotUI);
    }
    
    private void InitAccSlot(SO_Item.EAccType type)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_EquippedSlot", transform);
        UI_Inven_EquippedSlot equippedSlotUI = go.GetComponent<UI_Inven_EquippedSlot>();
        equippedSlotUI.InitOnce(SO_Item.EItemCategory.Acc, SO_Item.EArmorType.None, type);
        equippedAccSlots.Add(type, equippedSlotUI);
    }
    
    private void Init()
    {
        ClearItemSlots();
        List<SO_Item> items = GI.Inst.ListenerManager.GetEquippedItems();
        foreach (SO_Item item in items)
        {
            if (item.ItemCategory == SO_Item.EItemCategory.Weapon)
            {
                equippedWeaponSlot.SetItem(item, SO_Item.EItemCategory.Weapon);
            }
            else if (item.ItemCategory == SO_Item.EItemCategory.Armor)
            {
                SO_Item.EArmorType itemArmorType = ((SO_BaseArmor)item).armorType;
                if (equippedArmorSlots.ContainsKey(itemArmorType))
                {
                    equippedArmorSlots[itemArmorType].SetItem(item, SO_Item.EItemCategory.Armor);
                }
            }
            else if (item.ItemCategory == SO_Item.EItemCategory.Acc)
            {
                SO_Item.EAccType itemAccType = ((SO_BaseAcc)item).accType;
                if (equippedAccSlots.ContainsKey(itemAccType))
                {
                    equippedAccSlots[itemAccType].SetItem(item, SO_Item.EItemCategory.Acc);
                }
            }
        }
        // foreach (UI_Inven_EquippedSlot equippedSlot in equippedSlots)
        // {
        //     equippedSlot.SetItem(items);
        // }
    }

    private void ClearItemSlots()
    {
        equippedWeaponSlot.Clear();
        foreach (KeyValuePair<SO_Item.EArmorType,UI_Inven_EquippedSlot> pair in equippedArmorSlots.ToList())
        {
            pair.Value.Clear();
        }
        foreach (KeyValuePair<SO_Item.EAccType,UI_Inven_EquippedSlot> pair in equippedAccSlots.ToList())
        {
            pair.Value.Clear();
        }
    }

    public void RefreshEquippedUI()
    {
        Init();
    }
}