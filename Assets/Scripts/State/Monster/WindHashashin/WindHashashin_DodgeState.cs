using UnityEngine;

public class WindHashashin_DodgeState : MonsterState
{
    public WindHashashin_DodgeState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    private float DodgeDir { get; set; }
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDodge, true);
        DodgeDir = AIController.CurrentDir.x;
        Monster.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        Monster.SetVelocity(8f * DodgeDir, 0);
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isDodge, false);
    }
}
