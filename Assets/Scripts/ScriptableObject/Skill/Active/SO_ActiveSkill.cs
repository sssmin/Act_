using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EActiveSkillOrder
{
    First,
    Second,
    Third,
    Fourth,
    Fifth,
    Max
}

public class SO_ActiveSkill : SO_Skill
{
    public EActiveSkillOrder activeSkillOrder;

    public float[] coef = new float[10];

    public Define.EPlayerState skillState;
    
    //only charge skill
    public bool isChargeSkill;
    public float maxChargingTime = 2.5f;

    public void DataCopy(SO_ActiveSkill other)
    {
        DataCopy((SO_Skill)other);
        activeSkillOrder = other.activeSkillOrder;
        skillName = other.skillName;
        coef = other.coef;
        skillState = other.skillState;
        isChargeSkill = other.isChargeSkill;
        maxChargingTime = other.maxChargingTime;
    }
    
    public virtual void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        skillLevel = inSkillLevel;
    }
    
    public float GetSkillTotalDamage(float damage)
    {
        return Mathf.Round((damage * coef[skillLevel - 1] / 100f) * 10) * 0.1f;
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        Debug.Log("parent");
    }
}
