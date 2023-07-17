using UnityEngine;

public class WandererMagician_SphereAttackState : MonsterState
{
    public WandererMagician_SphereAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    public override void BeginState()
    {
        IsAnimTrigger = false;
        Animator.SetBool(AnimHash.isNormalAttack2, true);
        Monster.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;

            GameObject go = GI.Inst.ResourceManager.Instantiate("WandererMagicianProjectile",
                AIController.wallDetectObject.transform.position, Quaternion.identity);
            WanderMagicianProjectile wanderMagicianProjectile = go.GetComponent<WanderMagicianProjectile>();
            wanderMagicianProjectile.Init(AIController.CurrentDir, AIController, 1f);

        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isNormalAttack2, false);
    }
}
