using UnityEngine;

public class Player_FallingState : PlayerState
{
    public Player_FallingState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isFalling, true);
    }
    
    public override void Update()
    {
        base.Update();

        if (PlayerController.MoveDir.x != 0f)
            Player.SetVelocity(PlayerController.MoveDir.x * Player.MoveSpeed * 0.8f);

        if (PlayerController.IsWallDetect() && PlayerController.CheckStateCanWallSlide())
        {
            TransitionState(Define.EPlayerState.WallSliding);
            return;
        }
        if (PlayerController.IsPrepareJumpEnd())
        {
            TransitionState(Define.EPlayerState.JumpEnd);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isFalling, false);
    }
}

public class Player_JumpEndState : PlayerState
{
    public Player_JumpEndState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isJumpEnd, true);
    }
    
    public override void Update()
    {
        base.Update();
        
        if (PlayerController.MoveDir.x != 0f)
            Player.SetVelocity(PlayerController.MoveDir.x * Player.MoveSpeed * 0.8f);
        
        if (PlayerController.IsWallDetect() && PlayerController.CheckStateCanWallSlide())
        {
            TransitionState(Define.EPlayerState.WallSliding);
            return;
        }
        
        if (PlayerController.IsGroundDetect())
        {
            TransitionState(Define.EPlayerState.Idle);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isJumpEnd, false);
    }
}
