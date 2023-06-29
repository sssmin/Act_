using UnityEngine;

public class EarthquakeState : PlayerState
{
    public EarthquakeState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isEarthquake, true);
        Player.SetZeroVelocity();
        bIsAnimTrigger = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (bIsAnimTrigger)
        {
            bIsAnimTrigger = false;
            GI.Inst.ListenerManager.OnExecuteActiveSkill(Player.InstId, Define.ESkillId.Earthquake);
        }
       
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isEarthquake, false);
        bIsAnimTrigger = false;
    }
}