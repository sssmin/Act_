using System.Collections.Generic;
using UnityEngine;


public class UI_Inven_CategoryHolder : MonoBehaviour
{
    [SerializeField] UI_Inven_ItemSlotParent invenItemSlotParent;
    private const int MAX_INVEN_CATEGORY_NUM = 5;
    private List<UI_Inven_CategoryButton> invenCategoryButtons = new List<UI_Inven_CategoryButton>();
    
    public void InitOnce()
    {
        GI.Inst.UIManager.activeAllInvenCategoryBtn -= ActiveAllInvenCategoryBtn;
        GI.Inst.UIManager.activeAllInvenCategoryBtn += ActiveAllInvenCategoryBtn;
        GI.Inst.UIManager.onClickCategoryButton -= OnClickCategoryButton;
        GI.Inst.UIManager.onClickCategoryButton += OnClickCategoryButton;
        GI.Inst.UIManager.disableCategoryButtonCantEnhance -= DisableCategoryButtonCantEnhance;
        GI.Inst.UIManager.disableCategoryButtonCantEnhance += DisableCategoryButtonCantEnhance;
        GI.Inst.UIManager.enableCategoryButton -= EnableCategoryButton;
        GI.Inst.UIManager.enableCategoryButton += EnableCategoryButton;
        GI.Inst.UIManager.enableCanWeaponMat -= EnableCanWeaponMat;
        GI.Inst.UIManager.enableCanWeaponMat += EnableCanWeaponMat;
        
        
        for (int i = 0; i < MAX_INVEN_CATEGORY_NUM; i++)
        {
            int index = i;
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_CategoryButton", transform);
            UI_Inven_CategoryButton invenCategoryButton = go.GetComponent<UI_Inven_CategoryButton>();
            invenCategoryButton.InitOnce((SO_Item.EItemCategory)index); 
            if (index == 0)
                invenCategoryButton.DeactivateButton();
            invenCategoryButtons.Add(invenCategoryButton);
        }
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.activeAllInvenCategoryBtn -= ActiveAllInvenCategoryBtn;
        GI.Inst.UIManager.onClickCategoryButton -= OnClickCategoryButton;
        GI.Inst.UIManager.disableCategoryButtonCantEnhance -= DisableCategoryButtonCantEnhance;
        GI.Inst.UIManager.enableCategoryButton -= EnableCategoryButton;
        GI.Inst.UIManager.enableCanWeaponMat -= EnableCanWeaponMat;
    }

    private void ActiveAllInvenCategoryBtn()
    {
        foreach (UI_Inven_CategoryButton invenCategoryButton in invenCategoryButtons)
        {
            invenCategoryButton.ActivateButton();
        }
    }

    private void OnClickCategoryButton(SO_Item.EItemCategory itemCategory)
    {
        invenItemSlotParent.Init(itemCategory);
    }

    public void RefreshInventoryUI()
    {
        invenItemSlotParent.RefreshInventoryUI();
    }

    private void DisableCategoryButtonCantEnhance() //장비 강화 불가능한 카테고리 비활성화
    {
        foreach (UI_Inven_CategoryButton invenCategoryButton in invenCategoryButtons)
        {
            if (invenCategoryButton.itemCategory == SO_Item.EItemCategory.Weapon)
                continue;
            invenCategoryButton.DeactivateButton();
        }
    }

    private void EnableCategoryButton() //장비 강화 팝업 닫은 후 장비 카테고리만 비활성화
    {
        foreach (UI_Inven_CategoryButton invenCategoryButton in invenCategoryButtons)
        {
            if (invenCategoryButton.itemCategory == SO_Item.EItemCategory.Weapon)
                invenCategoryButton.DeactivateButton();
            else
                invenCategoryButton.ActivateButton();
        }
    }

    private void EnableCanWeaponMat(SO_BaseWeapon weapon) //재료로 가능한 장비만 활성화
    {
        invenItemSlotParent.EnableCanWeaponMat(weapon);
    }
}
