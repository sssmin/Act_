using System.Collections.Generic;
using UnityEngine;


public class UI_Inven_ItemSlotParent : MonoBehaviour
{
    private List<UI_Inven_ItemSlot> itemSlots = new List<UI_Inven_ItemSlot>();
    private List<UI_Inven_ConsumableSlot> consumableItemSlots = new List<UI_Inven_ConsumableSlot>();
    private SO_Item.EItemCategory _currentActiveItemCategory = SO_Item.EItemCategory.Weapon;

    public void Init(SO_Item.EItemCategory itemCategory)
    {
        _currentActiveItemCategory = itemCategory;
        ClearItemSlots();

        switch (itemCategory)
        {
            case SO_Item.EItemCategory.Weapon:
            case SO_Item.EItemCategory.Armor:
            case SO_Item.EItemCategory.Acc:
                List<SO_Item> items = GI.Inst.ListenerManager.GetItems(itemCategory);
                int itemNum = items.Count;
                if (itemNum == 0) return;
                foreach (SO_Item item in items)
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_ItemSlot", transform);
                    UI_Inven_ItemSlot invenItemSlot = go.GetComponent<UI_Inven_ItemSlot>();
                    invenItemSlot.SetItem(item);
                    itemSlots.Add(invenItemSlot);
                }
                break;

            case SO_Item.EItemCategory.Consumable:
                {
                    List<StackableItem> stackableItems = GI.Inst.ListenerManager.GetStackableItems(itemCategory);
                    foreach (StackableItem stackableItem in stackableItems)
                    {
                        foreach (int amount in stackableItem.amounts) //리스트 갯수만큼 slot 생성해야함.
                        {
                            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_ConsumableItemSlot", transform);
                            UI_Inven_ConsumableSlot invenConsumableSlot = go.GetComponent<UI_Inven_ConsumableSlot>();
                            invenConsumableSlot.SetStackableItem(stackableItem.item, amount);
                            
                            consumableItemSlots.Add(invenConsumableSlot);
                        }
                    }
                }
                break;
            case SO_Item.EItemCategory.Etc:
                {
                    List<StackableItem> stackableItems = GI.Inst.ListenerManager.GetStackableItems(itemCategory);
                    foreach (StackableItem stackableItem in stackableItems)
                    {
                        foreach (int amount in stackableItem.amounts) //리스트 갯수만큼 slot 생성해야함.
                        {
                            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_ItemSlot", transform);
                            UI_Inven_ItemSlot invenConsumableSlot = go.GetComponent<UI_Inven_ItemSlot>();
                            invenConsumableSlot.SetStackableItem(stackableItem.item, amount);
                            
                            itemSlots.Add(invenConsumableSlot);
                        }
                    }
                    
                }
                break;
        }
    }

    private void ClearItemSlots()
    {
        if (itemSlots.Count > 0)
        {
            for (int i = itemSlots.Count - 1; i >= 0; i--)
            {
                GI.Inst.ResourceManager.Destroy(itemSlots[i].gameObject);
            }
            itemSlots.Clear();
        }
        if (consumableItemSlots.Count > 0)
        {
            for (int i = consumableItemSlots.Count - 1; i >= 0; i--)
            {
                GI.Inst.ResourceManager.Destroy(consumableItemSlots[i].gameObject);
            }
            consumableItemSlots.Clear();
        }
    }

    public void RefreshInventoryUI()
    {
        Init(_currentActiveItemCategory);
    }

    public void EnableCanWeaponMat(SO_BaseWeapon weapon)
    {
        foreach (UI_Inven_ItemSlot itemSlot in itemSlots)
        {
            itemSlot.CheckDisableSlot(weapon);
        }
    }
    
    
    
}
