using System.Collections.Generic;
using UnityEngine;


public class UI_Inven_EquippedSlotParent : MonoBehaviour
{
    public List<UI_Inven_EquippedSlot> equippedSlots = new List<UI_Inven_EquippedSlot>();
    
    private void Init()
    {
        ClearItemSlots();
        List<SO_Item> items = GI.Inst.ListenerManager.GetEquippedItems();
        foreach (UI_Inven_EquippedSlot equippedSlot in equippedSlots)
        {
            equippedSlot.SetItem(items);
        }
    }

    private void ClearItemSlots()
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