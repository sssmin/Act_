using System;
using System.Collections.Generic;
using UnityEngine;

public enum EBindKeyMainCategory
{
    Skill,
    Item,
    Jump,
    Dash,
    NormalAttack,
    InventoryWindow,
    SkillWindow,
    Interact,
    
    Max
}

public class UI_Option_BindKeyContent : MonoBehaviour
{
    [SerializeField] private GameObject bindKeyPopup;
    [SerializeField] private Transform wrapperParent;

    private Dictionary<EBindKeyMainCategory, UI_Option_BindWrapper> BindWrappers { get; set; } =
        new Dictionary<EBindKeyMainCategory, UI_Option_BindWrapper>();

    public void InitOnce()
    {
        bindKeyPopup.SetActive(false);
        GI.Inst.UIManager.refreshBindKeyUI -= RefreshAll;
        GI.Inst.UIManager.refreshBindKeyUI += RefreshAll;

        for (int i = 0; i < (int)EBindKeyMainCategory.Max; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindWrapper", wrapperParent);
            UI_Option_BindWrapper bindWrapper = go.GetComponent<UI_Option_BindWrapper>();
            bindWrapper.Init((EBindKeyMainCategory)i);
            BindWrappers.Add((EBindKeyMainCategory)i, bindWrapper);
        }
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshBindKeyUI -= RefreshAll;
    }

    public void SetVisibleBindKeyPopup(bool isVisible)
    {
        bindKeyPopup.SetActive(isVisible);
    }
    

    private void RefreshAll()
    {
        foreach (KeyValuePair<EBindKeyMainCategory,UI_Option_BindWrapper> pair in BindWrappers)
        {
            pair.Value.RefreshAll();
        }
    }
}
