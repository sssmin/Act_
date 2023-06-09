using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScriptableObjectType : ScriptableObject
{
    public Define.EScriptableObjectType scriptableObjectType;
}

public class ActiveSkill : ScriptableObjectType
{
    public Define.ESkillId skillId;
    //public string skillId; //고유 id. 무기도 이 skillid를 동일하게 가진다.
    public float skillCooltime;
    public float baseDamage;
    public int skillLevel;
    [Range(1.8f, 2f)]
    public float minRangeCoef; //스킬마다 랜덤 계수 최소 범위
    [Range(1.8f, 2f)]
    public float maxRangeCoef; //스킬마다 랜덤 계수 최대 범위

    [HideInInspector]
    public float minCoef; //계산된 계수
    [HideInInspector]
    public float maxCoef;
    [HideInInspector]
    public float calcBaseDamage;
    public Define.EPlayerState skillState;
    
    //only charge skill
    public bool isChargeSkill;
    public float maxChargingTime = 2.5f;
    
    
    /*
     * RangeCoef = 랜덤 계수 최소 범위. 대미지 계산할 때 범위 내 랜덤으로 계수 적용
     * BaseDamage = 스킬 기본 베이스가 되는 대미지. 이것 또한 계수를 이용해서 레벨에 따라 증가
     * 스킬 대미지 적용 방식 : 스킬 베이스 대미지 + 플레이어 공격력의 100% + 계수 % 만큼 대미지 적용
     * 아래 minCoef, maxCoef를 이용해서 랜덤 계수
     * calcBaseDamage가 계산된 스킬 베이스 대미지
     * 또한 이 세 변수를 이용해서 스킬 툴팁에 대미지 정보 적용 => UI쪽에서 SkillManager랑 StatManager한테 요청해서 받아서 적용하도록 하자.
     * ex ) 대거를 던져 공격력의 118~120% + calcBaseDamage 피해를 입힙니다. 
     */


    public void Init(int inSkillLevel)
    {
        skillLevel = inSkillLevel;
        minCoef = 100 + skillLevel * minRangeCoef;
        maxCoef = 100 + skillLevel * maxRangeCoef;
        calcBaseDamage = baseDamage + (skillLevel * maxRangeCoef) * minRangeCoef;
    }

    public virtual void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        
    }

    public int GetSkillTotalDamage(int damage)
    {
        float coef = Random.Range(minCoef, maxCoef); //ex) 110~115
        return Mathf.RoundToInt(damage * coef / 100f + calcBaseDamage);
    }
}
