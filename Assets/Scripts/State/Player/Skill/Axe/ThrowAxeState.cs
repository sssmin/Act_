using UnityEngine;

public class ThrowAxeSkillState : PlayerState
{
    public ThrowAxeSkillState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isThrowAxeSkill, true);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }
    
    public override void Update()
    {
        base.Update();

        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteSkill(Player.InstId, Define.ESkillId.ThrowAxe);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isThrowAxeSkill, false);
        bIsAnimTrigger = false;
    }
}