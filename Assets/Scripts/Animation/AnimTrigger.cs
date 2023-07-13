using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    protected Animator Animator { get; set; }
    
    protected CombatManager CombatManager { get; set; }

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        
        CombatManager = GetComponentInParent<CombatManager>();
    }

    //나중에 이 클래스 베이스로 하고 플레이어, 몬스터 나누기.
    public virtual void GoToIdleState()
    {
       
    }

    public void OnSpawnArrow()
    {
        CombatManager.BowPrimaryAttack(); 
    }

    public virtual void OnExecSkillTrigger()
    {
    }

    //나중에 이 클래스 베이스로 하고 플레이어, 몬스터 나누기.
    public virtual void PauseAnimation()
    {
    }

    public void ResumeAnimation()
    {
        Animator.speed = 1f;
    }

    //나중에 이 클래스 베이스로 하고 플레이어, 몬스터 나누기.
    public virtual void SweepOverlapCircle()
    {
    }

    public virtual void ExecNormalAttackNotify()
    {
    }

    public virtual void NormalAttackCompleted()
    {
        
    }
    
    
}
