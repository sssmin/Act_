using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNormalAttackState : PlayerState
{
    public BaseNormalAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
        CombatManager = Player.CombatManager;
    }
    
    private float lastAttackTime;
    private float comboCooltime = 2f;

    private CombatManager CombatManager { get; set; }
    
    public override void BeginState()
    {
        if (CombatManager.CurrentComboNum % Define.MAX_NORMAL_ATTACK_COMBO_NUM == 0 || Time.time >= lastAttackTime + comboCooltime)
            CombatManager.InitCombo();
       
        Animator.SetInteger(AnimHash.attackComboNum, CombatManager.CurrentComboNum);
        Player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        IsAttacking = true;
        
        
        
    }

    public override void EndState()
    {
        IsAttacking = false;
        CombatManager.IncreaseCombo();
        lastAttackTime = Time.time;
    }
}
