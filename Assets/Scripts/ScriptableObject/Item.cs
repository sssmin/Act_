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

[CreateAssetMenu(fileName = "")]
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
