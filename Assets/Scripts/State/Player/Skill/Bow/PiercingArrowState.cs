using UnityEngine;

public class PiercingArrowState : PlayerState
{
    public PiercingArrowState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    private bool bIsExcuted;
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isPiercingArrowStart, true);
        Player.SetZeroVelocity();
        bIsExcuted = false;
    }
    
    public override void Update()
    {
        base.Update();
        
        if (bIsExcuted) return;
        
        if (PlayerController.IsCharging)
        {
            if (PlayerController.ChargeCompleted) //차징 중 차징이 다 됐으면
            {
                ExecuteSkill();
            }
        }
        else //차징하다가 키를 뗐으면
        {
            ExecuteSkill();
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isPiercingArrowStart, false);
        Animator.SetBool(AnimHash.isPiercingArrowShoot, false);
        bIsExcuted = false;
    }

    private void ExecuteSkill()
    {
        Animator.SetBool(AnimHash.isPiercingArrowShoot, true);
        bIsExcuted = true;
        PlayerController.PiercingArrowEnd();
        GI.Inst.ListenerManager.OnExecuteSkill(Player.InstId, Define.ESkillId.PiercingArrow);
    }
}