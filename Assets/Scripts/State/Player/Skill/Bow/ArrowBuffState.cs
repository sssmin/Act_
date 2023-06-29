using UnityEngine;

public class ArrowBuffState : PlayerState
{
    public ArrowBuffState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    
    public override void BeginState()
    {
        Player.SetZeroVelocity();
        GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.ArrowBuff);
        Player.TransitionState(Define.EPlayerState.Idle);
    }
    
    public override void Update()
    {
        base.Update();
    }

    public override void EndState()
    {
    }
}