using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_ItemHotkeySlotParent : MonoBehaviour
{
    Dictionary<SO_Item.EItemHotkeyOrder, UI_Main_ItemHotkeySlot> ItemHotkeySlots = new Dictionary<SO_Item.EItemHotkeyOrder, UI_Main_ItemHotkeySlot>();
    [SerializeField] private Image borderImage;
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshItemHotkeyUI -= InitItemHotkeySlot;
        GI.Inst.UIManager.refreshItemHotkeyUI += InitItemHotkeySlot;
        GI.Inst.UIManager.clearItemHotkeyUI -= ClearItemHotkeySlot;
        GI.Inst.UIManager.clearItemHotkeyUI += ClearItemHotkeySlot;

        for (int i = 0; i < (int)SO_Item.EItemHotkeyOrder.Max; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_ItemHotkeySlot", transform);
            UI_Main_ItemHotkeySlot mainItemHotkeySlot = go.GetComponent<UI_Main_ItemHotkeySlot>();
            ItemHotkeySlots.Add((SO_Item.EItemHotkeyOrder)i, mainItemHotkeySlot);
        }
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshItemHotkeyUI -= InitItemHotkeySlot;
        GI.Inst.UIManager.clearItemHotkeyUI -= ClearItemHotkeySlot;
    }

    private void InitItemHotkeySlot(SO_Item.EItemHotkeyOrder order, SO_Item item)
    {
        if (ItemHotkeySlots.ContainsKey(order))
        {
            ItemHotkeySlots[order].Init(item);
        }
    }
    
    private void ClearItemHotkeySlot(SO_Item.EItemHotkeyOrder order)
    {
        if (ItemHotkeySlots.ContainsKey(order))
        {
            ItemHotkeySlots[order].Clear();
        }
    }

    public void VisibleUI()
    {
        borderImage.color = new Color(0f, 0f, 0f, 130 / 255f);
        foreach (KeyValuePair<SO_Item.EItemHotkeyOrder,UI_Main_ItemHotkeySlot> pair in ItemHotkeySlots.ToList())
        {
            pair.Value.VisibleUI();
        }
    }

    public void InvisibleUI()
    {
        borderImage.color = new Color(0f, 0f, 0f, 0f);
        foreach (KeyValuePair<SO_Item.EItemHotkeyOrder,UI_Main_ItemHotkeySlot> pair in ItemHotkeySlots.ToList())
        {
            pair.Value.InvisibleUI();
        }
    }
}