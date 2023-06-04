using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeNormalAttackState : BaseNormalAttackState
{
    public AxeNormalAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        base.BeginState();
        Animator.SetBool(AnimHash.isAxeNormalAttack, true);
    }

    public override void Update()
    {
        base.Update();

        //todo ?
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isAxeNormalAttack, false);
    }
}
