using UnityEngine;

public class ThrowDaggerSkillState : PlayerState
{
    public ThrowDaggerSkillState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isThrowDaggerSkill, true);
        Player.SetZeroVelocity();
        IsAnimTrigger = false;
    }
    
    public override void Update()
    {
        base.Update();

        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.ThrowDagger);
        }
        
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isThrowDaggerSkill, false);
        IsAnimTrigger = false;
    }
}