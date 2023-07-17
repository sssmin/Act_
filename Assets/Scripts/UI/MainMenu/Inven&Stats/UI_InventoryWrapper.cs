using System;
using UnityEngine;


public class UI_InventoryWrapper : MonoBehaviour
{
    [SerializeField] private UI_Inven_CategoryHolder invenCategoryHolder;
    [SerializeField] private UI_Inven_GoldInvenCapacityParent invenGoldInvenCapacityParent;
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshGoldInvenCapacityUI -= RefreshGoldInvenCapacityUI;
        GI.Inst.UIManager.refreshGoldInvenCapacityUI += RefreshGoldInvenCapacityUI;
        
        invenCategoryHolder.InitOnce();
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshGoldInvenCapacityUI -= RefreshGoldInvenCapacityUI;
    }

    public void RefreshInventoryUI()
    {
        invenCategoryHolder.RefreshInventoryUI();
    }
    
    private void RefreshGoldInvenCapacityUI()
    {
        invenGoldInvenCapacityParent.RefreshGoldInvenCapacityUI();
    }
}
