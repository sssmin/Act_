using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    private Animator Animator { get; set; }
    private Player Player { get; set; }
    private CombatManager CombatManager { get; set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Player = GetComponentInParent<Player>();
        CombatManager = GetComponentInParent<CombatManager>();
    }

    //나중에 이 클래스 베이스로 하고 플레이어, 몬스터 나누기.
    public virtual void GoToIdleState()
    {
        if (Player)
            Player.TransitionState(Define.EPlayerState.Idle);
    }

    public void OnSpawnArrow()
    {
        CombatManager.BowPrimaryAttack(); 
    }

    public void OnExecSkillTrigger()
    {
        if (Player)
            GI.Inst.ListenerManager.OnTriggerAnim(Player.InstId);
    }

    //나중에 이 클래스 베이스로 하고 플레이어, 몬스터 나누기.
    public void PauseAnimation()
    {
        Animator.speed = 0f;
        Player.AnimPauseNotify();//State에 Trigger
    }

    public void ResumeAnimation()
    {
        Animator.speed = 1f;
    }

    //나중에 이 클래스 베이스로 하고 플레이어, 몬스터 나누기.
    public virtual void SweepOverlapCircle()
    {
        CombatManager.ExecuteNormalAttack("Monster");
    }

    public void DaggerUltCastingStart()
    {
        if (Player)
            GI.Inst.ListenerManager.OnTriggerAnim(Player.InstId);
    }
    
    
}
