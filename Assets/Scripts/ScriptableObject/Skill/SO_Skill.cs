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
    /*
     * RangeCoef = 랜덤 계수 최소 범위. 대미지 계산할 때 범위 내 랜덤으로 계수 적용
     * BaseDamage = 스킬 기본 베이스가 되는 대미지. 이것 또한 계수를 이용해서 레벨에 따라 증가
     * 스킬 대미지 적용 방식 : 스킬 베이스 대미지 + 플레이어 공격력의 100% + 계수 % 만큼 대미지 적용
     * 아래 minCoef, maxCoef를 이용해서 랜덤 계수
     * calcBaseDamage가 계산된 스킬 베이스 대미지
     * 또한 이 세 변수를 이용해서 스킬 툴팁에 대미지 정보 적용 => UI쪽에서 SkillManager랑 StatManager한테 요청해서 받아서 적용하도록 하자.
     * ex ) 대거를 던져 공격력의 118~120% + calcBaseDamage 피해를 입힙니다. 
     */
    

    public virtual void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        
    }

    
}
