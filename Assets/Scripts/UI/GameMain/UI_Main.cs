using UnityEngine;

public class UI_Main : MonoBehaviour
{
    [SerializeField] private UI_HealthBar healthBar;
    [SerializeField] private UI_Main_SkillHotkeySlotParent skillHotkeySlotParent;
    [SerializeField] private UI_Main_ItemHotkeySlotParent itemHotkeySlotParent;
    [SerializeField] private UI_Main_EffectSlotParent effectSlotParent;
    [SerializeField] private UI_HotkeyBarParent hotkeyBarParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] public RectTransform rectTransform;
    
    public void InitOnce()
    {
        skillHotkeySlotParent.InitOnce();
        itemHotkeySlotParent.InitOnce();
        hotkeyBarParent.InitOnce();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //canvas.worldCamera = GI.Inst.uiCamera;
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
