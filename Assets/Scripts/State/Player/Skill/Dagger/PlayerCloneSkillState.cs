using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneSkillState : PlayerState
{
    public PlayerCloneSkillState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDaggerCloneSkill, true);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteSkill(Player.InstId, Define.ESkillId.PlayerClone);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isDaggerCloneSkill, false);
        bIsAnimTrigger = false;
    }
}
