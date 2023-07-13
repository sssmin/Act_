
using UnityEngine;

public class AIController_WindHashashin : AIController_Boss
{
    
   
    public override void Start()
    {
        base.Start();
        
        NormalAttack2Cooltime = 8f;
        NormalAttack3Cooltime = 12f;
        SpecialAttack1Cooltime = 17f;
        DodgeCooltime = 5f;
        DefendCooltime = 3f;

        NormalAttack2Cooltimer = Time.time + NormalAttack2Cooltime;
        NormalAttack3Cooltimer = Time.time + NormalAttack3Cooltime;
        SpecialAttack1Cooltimer = Time.time + SpecialAttack1Cooltime;
        
    }
    


    public override void ExecSpecialAttack()
    {
        Collider2D[] colliders = new Collider2D[10];
        Physics2D.OverlapCircleNonAlloc(transform.position, 2f, colliders, LayerMask.GetMask("Player"));
        
        foreach (var collider in colliders)
        {
            if (collider == null) break;
            GameObject enemyGo = collider.gameObject;
            StatManager enemyStatManager = enemyGo.GetComponent<StatManager>();
            StatManager.CauseNormalAttack(enemyStatManager);
        }
    }

    public override void SpecialAttackCompleted()
    {

    }

    public void Teleport()
    {
        float rand = Random.Range(-0.3f, 0.3f);
        Vector3 targetPos = Target.transform.position;
        transform.position = new Vector3(targetPos.x + rand, transform.position.y, transform.position.z);
    }

    
}
