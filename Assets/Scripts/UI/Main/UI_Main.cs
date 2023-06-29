using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_Main : MonoBehaviour
{
    [SerializeField] private UI_HealthBar healthBar;
    [SerializeField] private UI_Main_SkillHotkeySlotParent skillHotkeySlotParent;
    [SerializeField] private UI_Main_ItemHotkeySlotParent itemHotkeySlotParent;
    [SerializeField] private UI_Main_EffectSlotParent effectSlotParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] public RectTransform rectTransform;
    
    public void InitOnce()
    {
        skillHotkeySlotParent.InitOnce();
        itemHotkeySlotParent.InitOnce();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GI.Inst.uiCamrea;
    }

    public void SetHpBar(float ratio)
    {
        healthBar.SetBar(ratio);
    }

    public void SetEffectSlot(EDurationEffectId effectId, Sprite icon)
    {
        effectSlotParent.SetEffectSlot(effectId, icon);
    }
    
    public void SetPassiveCooltimeSlot(Define.ESkillId skillId, Sprite icon)
    {
        effectSlotParent.SetPassiveSkil(skillId, icon);
    }


}
