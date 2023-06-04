using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EEquipmentType
{
    None,
    Necklace,
    Helmet,
    Glove,
    Armor,
    Boots,
    Weapon,
    
    Max
}

//스탯 
/*
 * 물리공격력
 * 물리방어력
 * 마법공격력
 * 마법저항력
 * 체력
 * 크리티컬 확률
 * 회피율
 * 스킬쿨타임증감율
 * 이동속도
 */


public class Equipment : ScriptableObject
{
    public EEquipmentType equipmentType;

    public int atk;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
