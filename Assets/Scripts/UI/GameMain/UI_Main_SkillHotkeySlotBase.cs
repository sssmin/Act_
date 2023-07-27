using UnityEngine;
using UnityEngine.UI;

public class UI_Main_SkillHotkeySlotBase : MonoBehaviour
{
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Sprite defaultSlotSprite;

    public virtual void InitOnce()
    {
    }
    
    public virtual void SetSkillIcon(Sprite icon)
    {
        skillIconImage.sprite = icon;
        
    }

    public virtual void Clear()
    {
        skillIconImage.sprite = defaultSlotSprite;
    }

    public virtual void VisibleUI()
    {
        skillIconImage.color = Color.white;
    }

    public virtual void InvisibleUI()
    {
        skillIconImage.color = Color.clear; 
    }
}
