using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill_EquipPassiveSkillSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int index;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Sprite defaultSlotIcon;
    [SerializeField] private Animator animator;
    private PassiveSkill_ShortVer skill;

    public void InitOnce()
    {
        skillIconImage.sprite = defaultSlotIcon;
        skill = new PassiveSkill_ShortVer();
        skill.skillId = Define.ESkillId.Max;
    }

    public void Init(PassiveSkill_ShortVer inSkill)
    {
        skillIconImage.sprite = inSkill.icon;
        skill = inSkill;
    }

    public void ClearIfSame(PassiveSkill_ShortVer inSkill)
    {
        if (skill.skillId == inSkill.skillId)
            Clear(skill.skillId);
    }
    
    private void Clear(Define.ESkillId skillId)
    {
        skill = new PassiveSkill_ShortVer();
        skill.skillId = Define.ESkillId.Max;
        skillIconImage.sprite = defaultSlotIcon;
        
        GI.Inst.ListenerManager.UnequipPassiveSkill(skillId);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GI.Inst.UIManager.BlinkEquipPassiveSkillSlot(false);
        if (eventData.pointerDrag != null)
        {
            if (skill.skillId != Define.ESkillId.Max)
                GI.Inst.ListenerManager.UnequipPassiveSkill(skill.skillId);
            UI_Skill_PassiveSkillSlot passiveSkillSlot =
                eventData.pointerDrag.gameObject.GetComponent<UI_Skill_PassiveSkillSlot>();
            
            PassiveSkill_ShortVer passiveSkill = passiveSkillSlot.PassiveSkill;
            GI.Inst.UIManager.CheckEquippedPassive(passiveSkill); //다른 칸 확인 후 같은 건 제거
            Init(passiveSkill);
            GI.Inst.ListenerManager.EquipPassiveSkill(passiveSkill.skillId, index);
        }
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            Clear(skill.skillId);
    }

    public void Blink(bool cond)
    {
        animator.SetBool(AnimHash.blink, cond);
    }
    
}
