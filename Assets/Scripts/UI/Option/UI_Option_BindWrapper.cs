using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Option_BindWrapper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText;
    private UI_Option_BindKeySlotParent bindKeySlotParent;
    
    public void Init(EBindKeyMainCategory type)
    {
        switch (type)
        {
            case EBindKeyMainCategory.Skill:
                typeText.text = "스킬";
                break;
            case EBindKeyMainCategory.Item:
                typeText.text = "아이템";
                break;
            case EBindKeyMainCategory.Jump:
                typeText.text = "점프";
                break;
            case EBindKeyMainCategory.Dash:
                typeText.text = "대쉬";
                break;
            case EBindKeyMainCategory.NormalAttack:
                typeText.text = "공격";
                break;
            case EBindKeyMainCategory.InventoryWindow:
                typeText.text = "인벤토리 창";
                break;
            case EBindKeyMainCategory.SkillWindow:
                typeText.text = "스킬 창";
                break;
            case EBindKeyMainCategory.Interact:
                typeText.text = "상호작용";
                break;
        }
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKeySlotParent", transform);
        bindKeySlotParent = go.GetComponent<UI_Option_BindKeySlotParent>();
        bindKeySlotParent.Init(type);
    }

    public void RefreshAll()
    {
        bindKeySlotParent.RefreshAll();
    }
}
