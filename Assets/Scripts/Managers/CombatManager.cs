using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int CurrentComboNum { get; private set; }
    public float NormalAttackCoefficient { get; private set; }
    private PlayerController PlayerController { get; set; }
    [SerializeField] public GameObject normalAttackOverlap;
    private float attackRange;
    
    //todo 나중에 추가할거, 무기 교체하면 Combo 초기화

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        CurrentComboNum = 0;
        NormalAttackCoefficient = 1f;
    }

    private void Start()
    {
        GI.Inst.ListenerManager.initCombo -= InitCombo;
        GI.Inst.ListenerManager.initCombo += InitCombo;
    }

    public void BowPrimaryAttack()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("Arrow", PlayerController.ControlledPlayer.arrowSpawnPoint.position, quaternion.identity);
        Arrow arrow = go.GetComponent<Arrow>();
        
        //todo 대미지 받아서 Init으로 넘겨줘야함 
       
        arrow.Init(PlayerController.AttackDir, inOwner: PlayerController, NormalAttackCoefficient); 
    }
    
    

    public void InitCombo()
    {
        CurrentComboNum = 0;
        NormalAttackCoefficient = 1.0f;
    }

    public void IncreaseCombo()
    {
        CurrentComboNum++;
        if (CurrentComboNum == 1)
            NormalAttackCoefficient = 1.15f;
        else if (CurrentComboNum == 2)
            NormalAttackCoefficient = 1.35f;
    }
    
    public void ExecuteNormalAttack(string layerNameForSearch)
    {
        Collider2D[] colliders = new Collider2D[10];
        Physics2D.OverlapCircleNonAlloc(normalAttackOverlap.transform.position, 0.3f, colliders, LayerMask.GetMask(layerNameForSearch));
      
        foreach (var collider in colliders)
        {
            if (collider == null) break;
            GameObject enemyGo = collider.gameObject;
            StatManager enemyStatManager = enemyGo.GetComponent<StatManager>();
            PlayerController.ControlledPlayer.StatManager.CauseNormalAttack(enemyStatManager, NormalAttackCoefficient);
        }
    }

    public static List<Transform> GetCloseEnemiesTransforms(Transform origin, int maxEnemyNum)
    {
        List<Transform> transforms = new List<Transform>();

        Collider2D[] collider2Ds = new Collider2D[maxEnemyNum];
        Physics2D.OverlapCircleNonAlloc(origin.position, 6f, collider2Ds);
        
        foreach (Collider2D col in collider2Ds)
        {
            if (col == null) break;
            if (col.CompareTag("Monster"))
            {
                transforms.Add(col.transform);
            }
        }
        
        return transforms;
    }
}
