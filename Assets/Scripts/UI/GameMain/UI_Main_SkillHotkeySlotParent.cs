using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_SkillHotkeySlotParent : MonoBehaviour
{
    private List<UI_Main_SkillHotkeySlotBase> SkillHotkeySlots = new List<UI_Main_SkillHotkeySlotBase>();
    [SerializeField] private Image borderImage;
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshSkillHotkeyMainUI -= Init;
        GI.Inst.UIManager.refreshSkillHotkeyMainUI += Init;
        GI.Inst.UIManager.setSkillCooltimeUI -= SetSkillCooltimeUI;
        GI.Inst.UIManager.setSkillCooltimeUI += SetSkillCooltimeUI;
        GI.Inst.UIManager.resetCooltimeUI -= ResetCooltimeUI;
        GI.Inst.UIManager.resetCooltimeUI += ResetCooltimeUI;
        GI.Inst.UIManager.clearActiveSkillHotkeySlots -= ClearSkillHotkeySlots;
        GI.Inst.UIManager.clearActiveSkillHotkeySlots += ClearSkillHotkeySlots;

        for (int i = 0; i < (int)EActiveSkillOrder.Max; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_SkillHotkeySlot", transform);
            UI_Main_SkillHotkeySlotBase mainSkillHotkeySlot = go.GetComponent<UI_Main_SkillHotkeySlot>();
            
            SkillHotkeySlots.Add(mainSkillHotkeySlot);
        }
        ClearSkillHotkeySlots();
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshSkillHotkeyMainUI -= Init;
        GI.Inst.UIManager.setSkillCooltimeUI -= SetSkillCooltimeUI;
        GI.Inst.UIManager.resetCooltimeUI -= ResetCooltimeUI;
        GI.Inst.UIManager.clearActiveSkillHotkeySlots -= ClearSkillHotkeySlots;
    }

    private void Init(List<Sprite> icons)
    {
        ClearSkillHotkeySlots();
        
        for (int i = 0; i < SkillHotkeySlots.Count; i++)
        {
            SkillHotkeySlots[i].SetSkillIcon(icons[i]);
        }
    }

    private void ClearSkillHotkeySlots()
    {
        foreach (UI_Main_SkillHotkeySlotBase skillHotkeySlot in SkillHotkeySlots)
        {
            skillHotkeySlot.Clear();
        }  
    }

    private void SetSkillCooltimeUI(EActiveSkillOrder order, float cooltime)
    {
        int index = (int)order;
        UI_Main_SkillHotkeySlot skillHotkeySlot = SkillHotkeySlots[index] as UI_Main_SkillHotkeySlot;
        skillHotkeySlot.SetCooltime(cooltime);
    }
    
    private void ResetCooltimeUI(EActiveSkillOrder order)
    {
        int index = (int)order;
        UI_Main_SkillHotkeySlot skillHotkeySlot = SkillHotkeySlots[index] as UI_Main_SkillHotkeySlot;
        skillHotkeySlot.ResetCooltimeUI();
    }
    
    
    public void VisibleUI()
    {
        borderImage.color = new Color(0f, 0f, 0f, 130 / 255f);
        foreach (UI_Main_SkillHotkeySlotBase hotkeySlot in SkillHotkeySlots)
        {
            hotkeySlot.VisibleUI();
        }
    }

    public void InvisibleUI()
    {
        borderImage.color = new Color(0f, 0f, 0f, 0f);
        foreach (UI_Main_SkillHotkeySlotBase hotkeySlot in SkillHotkeySlots)
        {
            hotkeySlot.InvisibleUI();
        }
    }
}
