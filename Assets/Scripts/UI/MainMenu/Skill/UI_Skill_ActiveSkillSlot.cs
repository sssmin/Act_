using UnityEngine;
using UnityEngine.EventSystems;

public struct ActiveSkill_ShortVer
{
    public Define.ESkillId skillId;
    public string skillName;
    public float skillCooltime;
    public Sprite icon;

    public int level;
    public bool bCanLevelUp;
    public EActiveSkillOrder activeSkillOrder;
    public ESkillMatId itemIdForLevelUp;
    public string skillDesc;

    public void DataCopy(SO_Skill skill, int inLevel)
    {
        SO_ActiveSkill activeSkill = skill as SO_ActiveSkill;
        if (activeSkill)
        {
            activeSkillOrder = activeSkill.activeSkillOrder;
        }
        skillId = skill.skillId;
        skillName = skill.skillName;
        skillCooltime = skill.skillCooltime;
        icon = skill.icon;

        level = inLevel;
        bCanLevelUp = skill.bCanLevelUp;
        itemIdForLevelUp = skill.itemIdForLevelUp;
        skillDesc = skill.SkillDesc;
    }
}

public class UI_Skill_ActiveSkillSlot : UI_Skill_BaseSkillSlot
{
    private ActiveSkill_ShortVer skill;
    
    [SerializeField] private RectTransform slotTransform;
    private int skillIndex = -1;
    
    public override void InitOnce()
    {
        base.InitOnce();
        skillUpButton.onClick.AddListener(OnClickLevelUpButton);
        Clear();
    }

    public void Refresh(ActiveSkill_ShortVer inSkill, int index)
    {
        skill = inSkill;
        skillIndex = index;
        skillIconImage.sprite = skill.icon;
        SetActiveObject(true);

        if (skill.skillId == Define.ESkillId.Dash)
        {
            SetActiveObject(false);
            return;
        }

        levelValueText.text = GI.Inst.ListenerManager.GetActiveSkillLevel(skill.activeSkillOrder).ToString();
        
        if (skill.bCanLevelUp)
            skillUpButton.interactable = true;
        else
            skillUpButton.interactable = false;
    }

    public override void OnClickLevelUpButton()
    {
        GI.Inst.ListenerManager.RequestActiveSkillLevelUp(skill, skillIndex);
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }

    public void Clear()
    {
        skill = new ActiveSkill_ShortVer();
        skillIndex = -1;
        SetActiveObject(false);
        skillIconImage.sprite = defaultSlotIcon;
    }

    protected override void BeginOverlapMouseOnIcon()
    {
        Vector3 goPos = gameObject.transform.position;
        Vector3 pos = new Vector3(goPos.x + slotTransform.rect.width, goPos.y);
        GI.Inst.UIManager.VisibleActiveSkillTooltip(skill, pos, Define.LEFT_PIVOT);
    }

 

}
