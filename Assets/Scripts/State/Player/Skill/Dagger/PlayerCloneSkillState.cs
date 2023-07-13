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
        IsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.PlayerClone);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isDaggerCloneSkill, false);
        IsAnimTrigger = false;
    }
}
