using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    [Header("Equipment")]
    [HideInInspector] public bool bIsEquipped;
    public ERarity rarity;
    
    public List<Stat> itemStats;
    public List<Effect> effects { get; set; } = new List<Effect>();
    public List<string> effectDescs { get; set; } = new List<string>();
    

    public virtual void Init(StatManager ownerStatManager)
    {
        
    }
    
}
