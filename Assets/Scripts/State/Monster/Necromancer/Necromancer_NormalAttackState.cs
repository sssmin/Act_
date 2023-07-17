using UnityEngine;

public class Necromancer_NormalAttackState : MonsterState
{
    public Necromancer_NormalAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    public override void BeginState()
    {
        IsAnimTrigger = false;
        Animator.SetBool(AnimHash.isNormalAttack1, true);
        Monster.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;

            GameObject go = GI.Inst.ResourceManager.Instantiate("NecromancerLightning",
                AIController.wallDetectObject.transform.position, Quaternion.identity);
            NecromancerLightning necromancerLightning = go.GetComponent<NecromancerLightning>();
            necromancerLightning.Init(AIController);

        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isNormalAttack1, false);
    }
}
