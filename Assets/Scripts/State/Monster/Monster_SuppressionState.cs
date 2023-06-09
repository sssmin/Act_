using UnityEngine;

//제압 상태
public class Monster_SuppressionState : MonsterState
{
    public Monster_SuppressionState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
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