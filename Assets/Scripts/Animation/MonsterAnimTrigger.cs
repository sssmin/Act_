using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimTrigger : AnimTrigger
{
    private Monster Monster { get; set; }

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

    public override void SweepOverlapCircle()
    {
        CombatManager.ExecuteNormalAttack("Player");
    }

    public void DestroyMySelf()
    {
        Monster.DestroyMySelf();
    }
}
