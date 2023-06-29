using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerUltState : PlayerState
{
    public DaggerUltState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDaggerUltBegin, true);
        Animator.SetFloat(AnimHash.daggerUltNum, 0);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            Animator.SetFloat(AnimHash.daggerUltNum, 1);//casting
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.DaggerUlt);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isDaggerUltBegin, false);
        bIsAnimTrigger = false;
    }
}
