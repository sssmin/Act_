using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.wallSlide, true);
    }
    
    public override void Update()
    {
        base.Update();

        if (!PlayerController.IsWallDetect())
        {
             TransitionState(Define.EPlayerState.Falling);
             return;
        }
        
        Rb.velocity = new Vector2(0, Rb.velocity.y * 0.7f);
        
        if (PlayerController.IsGroundDetect())
        { 
            TransitionState(Define.EPlayerState.Idle);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.wallSlide, false);
    }
}


