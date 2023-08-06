using UnityEngine;

public class AIController_Boss : AIController
{
    protected float NormalAttack2Cooltimer { get; set; }
    protected float NormalAttack3Cooltimer { get; set; }
    protected float SpecialAttack1Cooltimer { get; set; }
    protected float NormalAttack2Cooltime { get; set; }
    protected float NormalAttack3Cooltime { get; set; }
    protected float SpecialAttack1Cooltime { get; set; }
    
    private float DodgeCooltimer { get; set; }
    protected float DodgeCooltime { get; set; }
    private float DefendCooltimer { get; set; }
    protected float DefendCooltime { get; set; }

    public override void Start()
    {
        base.Start();

        GI.Inst.ListenerManager.playersNormalAttack -= TryToDodge;
        GI.Inst.ListenerManager.playersNormalAttack += TryToDodge;
        GI.Inst.ListenerManager.playersAttack -= IsDefend;
        GI.Inst.ListenerManager.playersAttack += IsDefend;
    }
    
    public override void OnTriggerExit2D(Collider2D col)
    {
        
    }

    public bool IsAttackReady(Define.EBossAttackType bossAttackType)
    {
        switch (bossAttackType)
        {
            case Define.EBossAttackType.NormalAttack2:
                return Time.time >= NormalAttack2Cooltimer;
            case Define.EBossAttackType.NormalAttack3:
                return Time.time >= NormalAttack3Cooltimer;
            case Define.EBossAttackType.SpecialAttack1:
                return Time.time >= SpecialAttack1Cooltimer;
        }

        return false;
    }

    public void SetAttackTimer(Define.EBossAttackType bossAttackType)
    {
        switch (bossAttackType)
        {
            case Define.EBossAttackType.NormalAttack2:
                NormalAttack2Cooltimer = Time.time + NormalAttack2Cooltime;
                break;
            case Define.EBossAttackType.NormalAttack3:
                NormalAttack3Cooltimer = Time.time + NormalAttack3Cooltime;
                break;
            case Define.EBossAttackType.SpecialAttack1:
                SpecialAttack1Cooltimer = Time.time + SpecialAttack1Cooltime;
                break;
        }
    }

    public virtual void ExecSpecialAttack()
    {
        
    }
    
    
    private void TryToDodge()
    {
        if (CanDodge())
        {
            float rand = Random.Range(0f, 100f);
            if (rand <= 20f)
            {
                ControlledMonster.TransitionState(Define.EMonsterState.Dodge);
                DodgeCooltimer = DodgeCooltime + Time.time;
            }
        }
    }

    private bool CanDodge()
    {
        return DodgeCooltimer <= Time.time;
    }

    private bool IsDefend()
    {
        if (CanDefend())
        {
            float rand = Random.Range(0f, 100f);
            if (rand <= 20f)
            {
                ControlledMonster.TransitionState(Define.EMonsterState.Defend);
                DefendCooltimer = DefendCooltime + Time.time;
                return true;
            }
        }

        return false;
    }

    private bool CanDefend()
    {
        return DefendCooltimer <= Time.time;
    }
}
