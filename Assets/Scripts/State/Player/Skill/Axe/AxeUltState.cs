using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeUltState : PlayerState
{
    public AxeUltState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isAxeUlt, true);
        Player.SetZeroVelocity();
        IsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (IsAnimTrigger)
        {
            Animator.SetBool(AnimHash.isAxeUltCasting, true);
            IsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.AxeUlt);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isAxeUlt, false);
        Animator.SetBool(AnimHash.isAxeUltCasting, false);
        IsAnimTrigger = false;
    }
}
