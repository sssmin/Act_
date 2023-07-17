using System.Collections.Generic;
using UnityEngine;

public class UI_Option_BindKeySlotParent : MonoBehaviour
{
    private Dictionary<EBindKeyType, UI_Option_BindKey> bindKeys = new Dictionary<EBindKeyType, UI_Option_BindKey>();

    public void Init(EBindKeyMainCategory type)
    {
        UI_Option_BindKey bindKey = null;
        switch (type)
        {
            case EBindKeyMainCategory.Skill:
                for (int i = (int)EBindKeyType.FirstSkill; i <= (int)EBindKeyType.FourthSkill; i++)
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init((EBindKeyType)i);
                    bindKeys.Add((EBindKeyType)i, bindKey);
                }
                return;
            case EBindKeyMainCategory.Item:
                for (int i = (int)EBindKeyType.FirstItemHotkey; i <= (int)EBindKeyType.FifthItemHotkey; i++)
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init((EBindKeyType)i);
                    bindKeys.Add((EBindKeyType)i, bindKey);
                }
                return;
            case EBindKeyMainCategory.Jump:
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init(EBindKeyType.Jump);
                }
                break;
            case EBindKeyMainCategory.Dash:
            {
                GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                bindKey = go.GetComponent<UI_Option_BindKey>();
                bindKey.Init(EBindKeyType.Dash);
            }
                break;
            case EBindKeyMainCategory.NormalAttack:
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init(EBindKeyType.NormalAttack);
                }
                break;
            case EBindKeyMainCategory.InventoryWindow:
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init(EBindKeyType.InventoryWindow);
                }
                break;
            case EBindKeyMainCategory.SkillWindow:
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init(EBindKeyType.SkillWindow);
                }
                break;
            case EBindKeyMainCategory.Interact:
                {
                    GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKey", transform);
                    bindKey = go.GetComponent<UI_Option_BindKey>();
                    bindKey.Init(EBindKeyType.Interaction);
                }
                break;
        }
        bindKeys.Add(EBindKeyType.Jump, bindKey);
    }

    public void RefreshAll()
    {
        foreach (KeyValuePair<EBindKeyType,UI_Option_BindKey> pair in bindKeys)
        {
            pair.Value.RefreshUI();
        }
    }
}
