using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Main_ItemHotkeySlotParent : MonoBehaviour
{
    Dictionary<Item.EItemHotkeyOrder, UI_Main_ItemHotkeySlot> ItemHotkeySlots = new Dictionary<Item.EItemHotkeyOrder, UI_Main_ItemHotkeySlot>();
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshItemHotkeyUI -= InitItemHotkeySlot;
        GI.Inst.UIManager.refreshItemHotkeyUI += InitItemHotkeySlot;
        GI.Inst.UIManager.clearItemHotkeyUI -= ClearItemHotkeySlot;
        GI.Inst.UIManager.clearItemHotkeyUI += ClearItemHotkeySlot;

        for (int i = 0; i < (int)Item.EItemHotkeyOrder.Max; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_ItemHotkeySlot", transform);
            UI_Main_ItemHotkeySlot mainItemHotkeySlot = go.GetComponent<UI_Main_ItemHotkeySlot>();
            ItemHotkeySlots.Add((Item.EItemHotkeyOrder)i, mainItemHotkeySlot);
        }
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshItemHotkeyUI -= InitItemHotkeySlot;
        GI.Inst.UIManager.clearItemHotkeyUI -= ClearItemHotkeySlot;
    }

    private void InitItemHotkeySlot(Item.EItemHotkeyOrder order, Item item)
    {
        if (ItemHotkeySlots.ContainsKey(order))
        {
            ItemHotkeySlots[order].Init(item);
        }
    }
    
    private void ClearItemHotkeySlot(Item.EItemHotkeyOrder order)
    {
        if (ItemHotkeySlots.ContainsKey(order))
        {
            ItemHotkeySlots[order].Clear();
        }
    }
}