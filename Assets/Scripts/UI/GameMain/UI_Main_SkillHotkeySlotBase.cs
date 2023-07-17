using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_SkillHotkeySlotBase : MonoBehaviour
{
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Sprite defaultSlotSprite;
    [SerializeField] private TextMeshProUGUI bindKeyText;

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
}
