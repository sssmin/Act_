using UnityEngine;

public class ArrowRainState : PlayerState
{
    public ArrowRainState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isArrowRain, true);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }
    
    public override void Update()
    {
        base.Update();

        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.ArrowRain);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isArrowRain, false);
        bIsAnimTrigger = false;
    }
}