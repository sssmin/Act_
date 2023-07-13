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
        Debug.Log("파개");
    }

    public void RefreshInventoryUI()
    {
        invenCategoryHolder.RefreshInventoryUI();
    }
    
    public void RefreshGoldInvenCapacityUI()
    {
        invenGoldInvenCapacityParent.RefreshGoldInvenCapacityUI();
    }
}
