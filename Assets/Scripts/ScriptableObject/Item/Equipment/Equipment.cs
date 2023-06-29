using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    [Header("Equipment")]
    [HideInInspector] public bool bIsEquipped;
    public ERarity rarity;
    public int EnhancementLevel { get; set; } = 1;
    
    public List<Stat> itemStats;
    public List<Effect> effects { get; set; } = new List<Effect>();
    public List<string> effectDescs { get; set; } = new List<string>();
    
    
    public override void DataCopy(Item item)
    {
        base.DataCopy(item);
        
        bIsEquipped = ((Equipment)item).bIsEquipped;
        itemStats = new List<Stat>();
        itemStats = ((Equipment)item).itemStats.ConvertAll(s => new Stat(s));
        rarity = ((BaseWeapon)item).rarity;
        
        EnhancementLevel = ((BaseWeapon)item).EnhancementLevel;
    }

    public virtual void Init(StatManager ownerStatManager)
    {
        
    }
    
}
