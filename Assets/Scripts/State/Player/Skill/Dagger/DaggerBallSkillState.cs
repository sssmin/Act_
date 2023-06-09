using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerBallSkillState : PlayerState
{
    public DaggerBallSkillState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDaggerBallSkill, true);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteSkill(Player.InstId, Define.ESkillId.DaggerBall);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isDaggerBallSkill, false);
        bIsAnimTrigger = false;
    }
}