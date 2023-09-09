using UnityEngine;

public enum EMainUIComponent
{
    HealthBar,
    EffectBar,
    HotkeyBar
}

public class UI_Main : MonoBehaviour
{
    [SerializeField] private UI_Main_HealthBar healthBar;
    [SerializeField] private UI_Main_EffectSlotParent effectSlotParent;
    [SerializeField] private UI_Main_HotkeyBarParent hotkeyBarParent;
    
    [SerializeField] private UI_Main_SkillHotkeySlotParent skillHotkeySlotParent;
    [SerializeField] private UI_Main_ItemHotkeySlotParent itemHotkeySlotParent;
    [SerializeField] private UI_Main_GetAnItemParent getAnItemParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] public RectTransform rectTransform;
    public FloatingJoystick FloatingJoystick { get; set; } 
    
    
    public void InitOnce()
    {
        skillHotkeySlotParent.InitOnce();
        itemHotkeySlotParent.InitOnce();
        hotkeyBarParent.InitOnce();
        getAnItemParent.InitOnce();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        FloatingJoystick = gameObject.GetComponentInChildren<FloatingJoystick>();
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

    public void VisibleMainUIComponent(EMainUIComponent mainUIComponent)
    {
        switch (mainUIComponent)
        {
            case EMainUIComponent.HealthBar:
                healthBar.VisibleUI();
                break;
            case EMainUIComponent.EffectBar:
                effectSlotParent.VisibleUI();
                break;
            case EMainUIComponent.HotkeyBar:
                hotkeyBarParent.VisibleUI();
                skillHotkeySlotParent.VisibleUI();
                itemHotkeySlotParent.VisibleUI();
                break;
        }
    }
    
    public void InvisibleMainUIComponent(EMainUIComponent mainUIComponent)
    {
        switch (mainUIComponent)
        {
            case EMainUIComponent.HealthBar:
                healthBar.InvisibleUI();
                break;
            case EMainUIComponent.EffectBar:
                effectSlotParent.InvisibleUI();
                break;
            case EMainUIComponent.HotkeyBar:
                hotkeyBarParent.InvisibleUI();
                skillHotkeySlotParent.InvisibleUI();
                itemHotkeySlotParent.InvisibleUI();
                break;
        }
    }

    public void VisibleAllMainUIComponent()
    {
        healthBar.VisibleUI();
        effectSlotParent.VisibleUI();
        hotkeyBarParent.VisibleUI();
        skillHotkeySlotParent.VisibleUI();
        itemHotkeySlotParent.VisibleUI();
    }
    
    public void InvisibleAllMainUIComponent()
    {
        healthBar.InvisibleUI();
        effectSlotParent.InvisibleUI();
        hotkeyBarParent.InvisibleUI();
        skillHotkeySlotParent.InvisibleUI();
        itemHotkeySlotParent.InvisibleUI();
    }

}
