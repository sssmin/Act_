

using UnityEngine;

public class MonsterAnimTrigger : AnimTrigger
{
    protected Monster Monster { get; set; }

    public override void Awake()
    {
        base.Awake();
        
        Monster = GetComponentInParent<Monster>();
    }

    public override void GoToIdleState()
    {
        if (Monster)
            Monster.TransitionState(Define.EMonsterState.Idle);
    }
    
    public override void OnExecSkillTrigger()
    {
        if (Monster)
            GI.Inst.ListenerManager.OnTriggerAnim(Monster.InstId);
    }
    
    public override void PauseAnimation()
    {
        Animator.speed = 0f;
        //Monster.AnimPauseNotify();//State에 Trigger
    }

    public override void ExecNormalAttackNotify()
    {
        Monster.AIController.ExecNormalAttack();
    }
    
    public void ExecSpecialAttackNotify()
    {
        ((AIController_Boss)Monster.AIController).ExecSpecialAttack();
    }

    public void ExecSpawningNormalAttack()
    {
        if (Monster)
            GI.Inst.ListenerManager.OnTriggerAnim(Monster.InstId);
    }
    
    public override void NormalAttackCompleted()
    {
        Monster.AIController.NormalAttackCompleted();
        GoToIdleState();
    }
    
    public void SpecialAttackCompleted()
    {
        ((AIController_Boss)Monster.AIController).SpecialAttackCompleted();
        GoToIdleState();
    }
    

    public void DestroyMySelf()
    {
        Monster.DestroyMySelf();
    }
}
