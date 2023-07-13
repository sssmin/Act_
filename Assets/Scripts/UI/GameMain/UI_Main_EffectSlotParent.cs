using System.Collections.Generic;
using UnityEngine;

public class UI_Main_EffectSlotParent : MonoBehaviour
{
    //private List<UI_Main_EffectSlot> effectSlots = new List<UI_Main_EffectSlot>();
    
    //스킬이나, 아이템의 부가효과 지속시간 표시
    public void SetEffectSlot(EDurationEffectId effectId, Sprite icon)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_EffectSlot", transform);
        UI_Main_EffectSlot effectSlot = go.GetComponent<UI_Main_EffectSlot>();
        effectSlot.transform.localScale = Vector3.one;
        effectSlot.InitDurationEffect(effectId, icon);
    }
    
    //패시브 스킬 쿨타임 표시
    public void SetPassiveSkil(Define.ESkillId skillId, Sprite icon)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_EffectSlot", transform);
        UI_Main_EffectSlot effectSlot = go.GetComponent<UI_Main_EffectSlot>();
        effectSlot.transform.localScale = Vector3.one;
        effectSlot.InitPassiveSkill(skillId, icon);
    }
    
}
