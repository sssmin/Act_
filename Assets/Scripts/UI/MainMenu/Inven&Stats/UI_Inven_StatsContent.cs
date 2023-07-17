using System;
using UnityEngine;

public class UI_Inven_StatsContent : MonoBehaviour
{
    private UI_InventoryWrapper inventoryWrapper;
    [SerializeField] private UI_Inven_EquippedSlotParent invenEquippedSlotParent;
    [SerializeField] private UI_Inven_StatSlotParent invenStatSlotParent;
    [SerializeField] private Transform borderTransform;
    
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshInventoryUI -= RefreshInventoryUI;
        GI.Inst.UIManager.refreshInventoryUI += RefreshInventoryUI;
        GI.Inst.UIManager.getInventoryWrapper -= GetInventoryWrapper;
        GI.Inst.UIManager.getInventoryWrapper += GetInventoryWrapper;
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_InventoryWrapper", borderTransform);
        inventoryWrapper = go.GetComponent<UI_InventoryWrapper>();
        inventoryWrapper.InitOnce();
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshInventoryUI -= RefreshInventoryUI;
        GI.Inst.UIManager.getInventoryWrapper -= GetInventoryWrapper;
    }

    private void RefreshInventoryUI()
    {
        inventoryWrapper.RefreshInventoryUI();
        invenEquippedSlotParent.RefreshEquippedUI();
        invenStatSlotParent.RefreshInventoryUI();
    }

    private UI_InventoryWrapper GetInventoryWrapper()
    {
        return inventoryWrapper;
    }

    public void AttachToInventory()
    {
        inventoryWrapper.transform.SetParent(borderTransform);
    }

    
}
