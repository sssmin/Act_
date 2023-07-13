using UnityEngine;

public class ThrowAxeSkillState : PlayerState
{
    public ThrowAxeSkillState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isThrowAxeSkill, true);
        Player.SetZeroVelocity();
        IsAnimTrigger = false;
    }
    
    public override void Update()
    {
        base.Update();

        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.ThrowAxe);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isThrowAxeSkill, false);
        IsAnimTrigger = false;
    }
}