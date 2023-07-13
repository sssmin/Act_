using UnityEngine;

public class FireStrikeState : PlayerState
{
    public FireStrikeState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isFireStrike, true);
        Player.SetZeroVelocity();
        IsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.FireStrike);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isFireStrike, false);
        IsAnimTrigger = false;
    }
}
