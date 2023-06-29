using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Skill_BaseSkillSlot : MonoBehaviour
{
    [SerializeField] protected UI_SkillIcon skillIconUI;
    [SerializeField] protected Image skillIconImage;
    [SerializeField] protected Button skillUpButton;
    [SerializeField] protected TextMeshProUGUI levelValueText;
    [SerializeField] protected GameObject lvObject; //Lv. Text Only object
    [SerializeField] protected Sprite defaultSlotIcon;
    
    public virtual void InitOnce()
    {
        skillIconUI.InitOnce(beginCallback: BeginOverlapMouseOnIcon, endCallback: EndOverlapMouseOnIcon);
    }
    
    public virtual void OnClickLevelUpButton()
    {
    }
    
    protected void SetActiveObject(bool condi)
    {
        skillUpButton.gameObject.SetActive(condi);
        lvObject.SetActive(condi);
        levelValueText.gameObject.SetActive(condi);
    }
    
    protected virtual void BeginOverlapMouseOnIcon()
    {
    }
    
    protected void EndOverlapMouseOnIcon()
    {
        GI.Inst.UIManager.InvisibleSkillTooltip();
    }
}
