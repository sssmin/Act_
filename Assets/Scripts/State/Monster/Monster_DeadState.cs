using UnityEngine;

public class Monster_DeadState : MonsterState
{
    public Monster_DeadState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        AnimatorControllerParameter[] parameters = Animator.parameters;

        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                Animator.SetBool(parameter.name, false);
            }
        }
        Monster.SetZeroVelocity();
        Animator.SetBool(AnimHash.isDead, true);
    }
    
    public override void EndState()
    {
        
    }
    
    
}