using System.Collections.Generic;
using UnityEngine;

public class UI_Main_SkillHotkeySlotParent : MonoBehaviour
{
    private List<UI_Main_SkillHotkeySlotBase> SkillHotkeySlots = new List<UI_Main_SkillHotkeySlotBase>();
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshSkillHotkeyMainUI -= Init;
        GI.Inst.UIManager.refreshSkillHotkeyMainUI += Init;
        GI.Inst.UIManager.setSkillCooltimeUI -= SetSkillCooltimeUI;
        GI.Inst.UIManager.setSkillCooltimeUI += SetSkillCooltimeUI;
        GI.Inst.UIManager.resetCooltimeUI += ResetCooltimeUI;
        GI.Inst.UIManager.resetCooltimeUI += ResetCooltimeUI;
        GI.Inst.UIManager.clearActiveSkillHotkeySlots -= ClearSkillHotkeySlots;
        GI.Inst.UIManager.clearActiveSkillHotkeySlots += ClearSkillHotkeySlots;
        GI.Inst.UIManager.updateFillAmount -= UpdateUltFillAmount;
        GI.Inst.UIManager.updateFillAmount += UpdateUltFillAmount;
        
        for (int i = 0; i < (int)EActiveSkillOrder.Max; i++)
        {
            UI_Main_SkillHotkeySlotBase mainSkillHotkeySlot = null;
            
            if (i == (int)EActiveSkillOrder.Fourth)
            {
                GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_UltHotkeySlot", transform);
                mainSkillHotkeySlot = go.GetComponent<UI_Main_UltHotkeySlot>();
                mainSkillHotkeySlot.InitOnce();
            }
            else
            {
                GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_SkillHotkeySlot", transform);
                mainSkillHotkeySlot = go.GetComponent<UI_Main_SkillHotkeySlot>();
            }
            
            SkillHotkeySlots.Add(mainSkillHotkeySlot);
        }
        ClearSkillHotkeySlots();
    }

    public void Init(List<Sprite> icons)
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

    public void SetSkillCooltimeUI(EActiveSkillOrder order, float cooltime)
    {
        int index = (int)order;
        UI_Main_SkillHotkeySlot skillHotkeySlot = SkillHotkeySlots[index] as UI_Main_SkillHotkeySlot;
        skillHotkeySlot.SetCooltime(cooltime);
    }
    
    public void ResetCooltimeUI(EActiveSkillOrder order)
    {
        int index = (int)order;
        UI_Main_SkillHotkeySlot skillHotkeySlot = SkillHotkeySlots[index] as UI_Main_SkillHotkeySlot;
        skillHotkeySlot.ResetCooltimeUI();
    }
    
    public void UpdateUltFillAmount(float chargeAmount)
    {
        foreach (UI_Main_SkillHotkeySlotBase skillHotkeySlot in SkillHotkeySlots)
        {
            UI_Main_UltHotkeySlot ultHotkeySlot = skillHotkeySlot as UI_Main_UltHotkeySlot;
            if (ultHotkeySlot)
            {
                ultHotkeySlot.UpdateFillAmount(chargeAmount);
            }
        }  
    }
}
