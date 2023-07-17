using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct PassiveSkill_ShortVer
{
    public Define.ESkillId skillId;
    public string skillName;
    public float skillCooltime;
    public Sprite icon;
    
    public int level;
    public bool bCanLevelUp;
    public ESkillMatId itemIdForLevelUp;
    public string skillDesc;
    
    public void DataCopy(SO_Skill skill)
    {
        SO_PassiveSkill passiveSkill = skill as SO_PassiveSkill;
        
        skillId = skill.skillId;
        skillName = skill.skillName;
        skillCooltime = skill.skillCooltime;
        icon = skill.icon;
        
        level = passiveSkill.skillLevel;
        bCanLevelUp = skill.bCanLevelUp;
        itemIdForLevelUp = skill.itemIdForLevelUp;
        skillDesc = skill.SkillDesc;
    }
}


public class UI_Skill_PassiveSkillSlot : UI_Skill_BaseSkillSlot, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public PassiveSkill_ShortVer PassiveSkill;
    [SerializeField] private RectTransform slotTransform;
    private GameObject moveSkillIconGo = null;

    public override void InitOnce()
    { 
        base.InitOnce();
        skillUpButton.onClick.RemoveListener(OnClickLevelUpButton);
        skillUpButton.onClick.AddListener(OnClickLevelUpButton);
        SetActiveObject(false);
    }

    private void OnDestroy()
    {
        skillUpButton.onClick.RemoveListener(OnClickLevelUpButton);
    }

    public void Refresh(PassiveSkill_ShortVer inSkill)
    {
        PassiveSkill = inSkill;
        skillIconImage.sprite = PassiveSkill.icon;
        SetActiveObject(true);

        levelValueText.text = PassiveSkill.level.ToString();
        
        if (PassiveSkill.bCanLevelUp)
            skillUpButton.interactable = true;
        else
            skillUpButton.interactable = false;
    }
    
    public override void OnClickLevelUpButton()
    {
        GI.Inst.ListenerManager.RequestPassiveSkillLevelUp(ref PassiveSkill);
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        moveSkillIconGo =
            GI.Inst.ResourceManager.Instantiate("UI_Skill_MoveSkillIcon", GI.Inst.UIManager.MainMenuUI.transform);
        moveSkillIconGo.transform.position = skillIconImage.transform.position;
        moveSkillIconGo.GetComponent<Image>().sprite = PassiveSkill.icon;
        
        GI.Inst.UIManager.BlinkEquipPassiveSkillSlot(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        moveSkillIconGo.transform.position = eventData.position;
    }
    
    //EquipSlot OnDrop 먼저 호출되고 EndDrag 호출됨
    public void OnEndDrag(PointerEventData eventData)
    {
        GI.Inst.ResourceManager.Destroy(moveSkillIconGo);
        GI.Inst.UIManager.BlinkEquipPassiveSkillSlot(false);
    }

    protected override void BeginOverlapMouseOnIcon()
    {
        Vector3 goPos = gameObject.transform.position;
        Vector3 pos = new Vector3(goPos.x - slotTransform.rect.width, goPos.y);
        GI.Inst.UIManager.VisiblePassiveSkillTooltip(PassiveSkill, pos, Define.RIGHT_PIVOT);
    }

  
    
}
