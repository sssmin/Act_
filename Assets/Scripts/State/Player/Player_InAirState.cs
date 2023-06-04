using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InAirState : PlayerState
{
    public Player_InAirState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    public override void BeginState()
    {
        Rb.velocity = new Vector2(Rb.velocity.x, Player.JumpForce);
        Animator.SetBool(AnimHash.isInAir, true);
    }

    public override void Update()
    {
        base.Update();
        
        if (PlayerController.MoveDir.x != 0f)
            Player.SetVelocity(PlayerController.MoveDir.x * Player.MoveSpeed * 0.8f);
        
        if (PlayerController.IsWallDetect())
        {
            TransitionState(Define.EPlayerState.WallSliding);
            return;
        }
        
        if (Rb.velocity.y < 0)
        {
            TransitionState(Define.EPlayerState.Falling);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isInAir, false);
    }
}
