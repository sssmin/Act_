using UnityEngine;


public class SO_Skill : ScriptableObject
{
    public Define.ESkillId skillId;
    public string skillName;
    public float skillCooltime;
    [HideInInspector] public int skillLevel;
    public Sprite icon;
    [HideInInspector] public bool bCanLevelUp;
    public ESkillMatId itemIdForLevelUp;
    public string SkillDesc { get; protected set; }

    public void DataCopy(SO_Skill other)
    {
        skillId = other.skillId;
        skillName = other.skillName;
        skillCooltime = other.skillCooltime;
        skillLevel = other.skillLevel;
        icon = other.icon;
        itemIdForLevelUp = other.itemIdForLevelUp;
        SkillDesc = other.SkillDesc;
    }
  
    
    public virtual void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        
    }

    
}
