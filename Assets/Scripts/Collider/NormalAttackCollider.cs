using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackCollider : MonoBehaviour
{
    private BoxCollider2D NormalAttackBoxCollider;
    private CombatManager CombatManager { get; set; }
    private bool CanTrigger { get; set; }

    private List<StatManager> EnemyStatManagers { get; set; } = new List<StatManager>();
    
    void Awake()
    {
        NormalAttackBoxCollider = GetComponent<BoxCollider2D>();
        CombatManager = GetComponentInParent<CombatManager>();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EnemyStatManagers.Clear();
    }
    

   private void OnTriggerStay2D(Collider2D col)
   {
       GameObject enemyGo = col.gameObject;
       StatManager enemyStatManager = enemyGo.GetComponent<StatManager>();
       if (enemyStatManager)
       {
           foreach (StatManager statManager in EnemyStatManagers)
           {
               if (enemyStatManager == statManager) return;
           }
           EnemyStatManagers.Add(enemyStatManager);
           CombatManager.NormalAttackSuccessful(enemyStatManager);
       }
       
   }

    public void SetIsTrigger(bool cond)
    {
        CanTrigger = cond;
    }
}
