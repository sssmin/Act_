using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnhanceValueByRarity
{
    [SerializeField] public SO_Item.ERarity rarity;
    [SerializeField] public List<int> enhanceValue;
}

[CreateAssetMenu(fileName = "WeaponEnhanceValueByLevel", menuName ="Data/WeaponEnhanceValueByLevel")]
public class SO_WeaponEnhanceValueByLevel : ScriptableObject
{
    [SerializeField] public List<EnhanceValueByRarity> enhanceValueByRarities = new List<EnhanceValueByRarity>();
    
}
