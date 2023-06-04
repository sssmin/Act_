using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_FreezeState : MonsterState
{
    public Monster_FreezeState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }

    public override void BeginState()
    {
        Monster.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void EndState()
    {
    }
}