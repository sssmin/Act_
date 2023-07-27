using UnityEngine;

public class WindHashashin_Tornado : MonsterState
{
    public WindHashashin_Tornado(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
        aiControllerWindHashashin = (AIController_WindHashashin)AIController;
    }

    private AIController_WindHashashin aiControllerWindHashashin;
    
    public override void BeginState()
    {
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isNormalAttack3, true);
        Monster.SetZeroVelocity();
        aiControllerWindHashashin.SetAttackTimer(Define.EBossAttackType.NormalAttack3);
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimTrigger)
        {
            IsAnimTrigger = false;

            GameObject go = GI.Inst.ResourceManager.Instantiate("Tornado",
                AIController.wallDetectObject.transform.position, Quaternion.identity);
            Tornado tornado = go.GetComponent<Tornado>();
            tornado.Init(AIController.CurrentDir, AIController, 1.5f);

        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isNormalAttack3, false);
    }
}
