using System.Collections;
using UnityEngine;

public class Arrow : BaseProjectile
{
    
    public override void Init(Vector2 dir, BaseController inOwner, float inAttackCoefficient)
    {
        base.Init(dir, inOwner, inAttackCoefficient);

        StartCoroutine(CoDestroyMyself(3f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StatManager enemyStatManager = other.GetComponent<StatManager>();
        PlayerController PC = OwnerController as PlayerController;
        PC.ControlledPlayer.StatManager.CauseNormalAttack(enemyStatManager, AttackCoefficient);
        
        GI.Inst.ResourceManager.Destroy(gameObject);
    }

    IEnumerator CoDestroyMyself(float second)
    {
        yield return new WaitForSeconds(second);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
