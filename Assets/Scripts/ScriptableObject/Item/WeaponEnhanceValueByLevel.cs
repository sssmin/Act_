using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnhanceValueByRarity
{
    [SerializeField] public Item.ERarity rarity;
    [SerializeField] public List<int> enhanceValue;
}

[CreateAssetMenu(fileName = "WeaponEnhanceValueByLevel", menuName ="Data/WeaponEnhanceValueByLevel")]
public class WeaponEnhanceValueByLevel : ScriptableObject
{
    [SerializeField] public List<EnhanceValueByRarity> enhanceValueByRarities = new List<EnhanceValueByRarity>();
    
}
