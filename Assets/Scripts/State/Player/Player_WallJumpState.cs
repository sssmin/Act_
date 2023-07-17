using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    private float wallJumpTimer;
    
    public override void BeginState()
    {
        Rb.velocity = new Vector2(PlayerController.MoveDir.x * -3f, Player.JumpForce);
        
        Animator.SetBool(AnimHash.isInAir, true);
        wallJumpTimer = 0.4f;
    }
    
    
    public override void Update()
    {
        base.Update();

        wallJumpTimer -= Time.deltaTime;
        
        if (wallJumpTimer < 0f)
        {
            TransitionState(Define.EPlayerState.Falling);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isInAir, false);
    }
}