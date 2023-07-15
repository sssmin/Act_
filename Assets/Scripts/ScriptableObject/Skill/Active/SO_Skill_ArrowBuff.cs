using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/ArrowBuff")]
public class SO_Skill_ArrowBuff : SO_ActiveSkill
{
    private DurationEffect durationEffect;
    private int perDmg;
    private EffectInfo effectInfo;
    private float duration;
    
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);

        perDmg = GI.Inst.ListenerManager.GetActiveSkillLevel(activeSkillOrder) * 2;
        
        effectInfo = new EffectInfo();

        effectInfo.onExecuteIncreaseStat = () =>
        {
            casterStatManager.characterStats.normalAttackDamageIncPer.AddModifier(perDmg);
        };
        
        effectInfo.onExecuteDecreaseStat = () =>
        {
            casterStatManager.characterStats.normalAttackDamageIncPer.SubModifier(perDmg);
        };
        duration = 10f;
        
        SkillDesc = $"기본 공격 피해량을 {duration}초 동안 {perDmg}% 증가시킵니다.";
        
        durationEffect = new DurationEffect();
        
        durationEffect.Init(
            Define.EActivationCondition.None, 
            -1f, 
            Define.EDamageType.None, 
            effectInfo, 
            duration, 
            EDurationEffectId.ArrowBuff,
            true);
    }
    
    public override void ExecSkill(StatManager casterStatManager, PlayerController playerController)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("ArrowBuff", playerController.transform.position,
                Quaternion.identity);
        GI.Inst.ResourceManager.Destroy(go, 2f);

        float endTime = duration + Time.time;
        durationEffect.durationEndTime = endTime;
        
        
        casterStatManager.ExecDurationEffect(durationEffect, icon);
    }
}