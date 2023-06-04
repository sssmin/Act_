using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveState : PlayerState
{
    public Player_MoveState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController) 
        : base(animator, rigidbody2D, character, baseController) { }

    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isMove, true);
        Animator.SetFloat(AnimHash.xVelocity, PlayerController.MoveDir.x);
        Player.SetVelocity(PlayerController.MoveDir.x * Player.MoveSpeed);
    }

    public override void Update()
    {
        base.Update();

        
        if (PlayerController.IsWallDetect())
        {
            TransitionState(Define.EPlayerState.Idle);
            return;
        }

        if (!PlayerController.IsGroundDetect())
        {
            TransitionState(Define.EPlayerState.Falling);
            return;
        }
        
        Player.SetVelocity(PlayerController.MoveDir.x * Player.MoveSpeed);

        if (PlayerController.MoveDir.x == 0f)
        {
            TransitionState(Define.EPlayerState.Idle);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isMove, false);
    }
}
