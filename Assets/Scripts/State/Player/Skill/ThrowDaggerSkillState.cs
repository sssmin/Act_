using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDaggerSkillState : PlayerState
{
    public ThrowDaggerSkillState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isThrowDaggerSkill, true);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }
    
    public override void Update()
    {
        base.Update();

        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteSkill(Player.InstId, Define.SkillId.ThrowDagger);
        }
        
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isThrowDaggerSkill, false);
        bIsAnimTrigger = false;
    }
}