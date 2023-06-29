using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Inven_EquippedSlotParent : MonoBehaviour
{
    public List<UI_Inven_EquippedSlot> equippedSlots = new List<UI_Inven_EquippedSlot>();
    
    public void Init()
    {
        ClearItemSlots();
        List<Item> items = GI.Inst.ListenerManager.GetEquippedItems();
        foreach (UI_Inven_EquippedSlot equippedSlot in equippedSlots)
        {
            equippedSlot.SetItem(items);
        }
    }

    public void ClearItemSlots()
    {
        foreach (UI_Inven_EquippedSlot equippedSlot in equippedSlots)
        {
            equippedSlot.Clear();
        }
    }

    public void RefreshEquippedUI()
    {
        Init();
    }
}