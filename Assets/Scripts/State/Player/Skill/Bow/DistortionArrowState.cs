using UnityEngine;

public class DistortionArrowState : PlayerState
{
    public DistortionArrowState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDistortionArrow, true);
        Player.SetZeroVelocity();
        IsAnimTrigger = false;
    }
    
    public override void Update()
    {
        base.Update();

        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.DistortionArrow);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isDistortionArrow, false);
        IsAnimTrigger = false;
    }
}