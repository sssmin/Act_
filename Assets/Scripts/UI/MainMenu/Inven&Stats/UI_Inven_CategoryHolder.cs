using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class UI_Inven_CategoryHolder : MonoBehaviour
{
    [SerializeField] UI_Inven_ItemSlotParent invenItemSlotParent;
    private const int MAX_INVEN_CATEGORY_NUM = 5;
    private List<UI_Inven_CategoryButton> invenCategoryButtons = new List<UI_Inven_CategoryButton>();
    
    public void InitOnce()
    {
        GI.Inst.UIManager.activeAllInvenCategoryBtn -= ActiveAllInvenCategoryBtn;
        GI.Inst.UIManager.activeAllInvenCategoryBtn += ActiveAllInvenCategoryBtn;
        GI.Inst.UIManager.onClickCategoryButton -= InitItemSlots;
        GI.Inst.UIManager.onClickCategoryButton += InitItemSlots;
        
        for (int i = 0; i < MAX_INVEN_CATEGORY_NUM; i++)
        {
            int index = i;
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_CategoryButton", transform);
            UI_Inven_CategoryButton invenCategoryButton = go.GetComponent<UI_Inven_CategoryButton>();
            invenCategoryButton.Init((Item.EItemCategory)index); 
            if (index == 0)
                invenCategoryButton.DeactivateButton();
            invenCategoryButtons.Add(invenCategoryButton);
        }
    }

    public void ActiveAllInvenCategoryBtn()
    {
        foreach (UI_Inven_CategoryButton invenCategoryButton in invenCategoryButtons)
        {
            invenCategoryButton.ActivateButton();
        }
    }

    public void InitItemSlots(Item.EItemCategory itemCategory)
    {
        invenItemSlotParent.Init(itemCategory);
    }

    public void RefreshInventoryUI()
    {
        invenItemSlotParent.RefreshInventoryUI();
    }
    
}
