using System.Collections;
using UnityEngine;

public class WanderMagicianProjectile : BaseProjectile
{
    public override void Init(Vector2 dir, BaseController inOwner, float inAttackCoefficient)
    {
        base.Init(dir, inOwner, inAttackCoefficient);

        StartCoroutine(CoDestroyMyself(3f));
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        StatManager enemyStatManager = other.GetComponent<StatManager>();
        if (enemyStatManager)
        {
            AIController aiController = OwnerController as AIController;
            aiController.ControlledMonster.StatManager.CauseNormalAttack(enemyStatManager, AttackCoefficient);
        
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    IEnumerator CoDestroyMyself(float second)
    {
        yield return new WaitForSeconds(second);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
