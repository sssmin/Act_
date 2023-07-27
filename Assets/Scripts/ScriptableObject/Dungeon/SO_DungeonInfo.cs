using System;
using System.Collections.Generic;
using Serializable;
using UnityEngine;

public enum EDungeonType
{
    TempestTomb, //WindHashashin
    WhirlwindCatacomb, //WindHashashin2
    None,
    
    Max
}

public enum EDungeonCategory
{
    Normal,
    Boss
}

[Serializable]
public class DungeonRewardItemInfo
{
    [SerializeField] public string itemId;
    [SerializeField] public int amount;
    [SerializeField] public SO_Item.EItemCategory itemCategory;
    [SerializeField] public float dropChance;
}

[Serializable]
public class DungeonInfoDetail
{
    [SerializeField] public EDungeonCategory dungeonCategory;
    [SerializeField] public int dungeonLevel;
    [SerializeField] public List<string> spawnMonsterIds;
    [SerializeField] public int monsterLevel;
    [SerializeField] public List<DungeonRewardItemInfo> rewardItemIds;
    [HideInInspector] public bool isLevelCompleted;
}

[CreateAssetMenu(fileName = "DungeonInfo", menuName ="Data/DungeonInfo")]
public class SO_DungeonInfo : ScriptableObject
{
    [SerializeField] public EDungeonType dungeonType;
    [SerializeField] public string desc;
    [SerializeField] public List<DungeonInfoDetail> dungeonInfoDetails;
    [HideInInspector] public bool isDungeonCompleted;

    public static string GetDungeonName(EDungeonType type)
    {
        switch (type)
        {
            case EDungeonType.TempestTomb:
                return "템페스트 툼";
            case EDungeonType.WhirlwindCatacomb:
                return "휠윈드 카타콤";
            case EDungeonType.None:
                return "...";
        }

        return "";
    }

    public void CopyFromLite(DungeonInfo_Lite dungeonInfoLite)
    {
        dungeonType = dungeonInfoLite.dungeonType;
        desc = dungeonInfoLite.desc;
        dungeonInfoDetails = dungeonInfoLite.dungeonInfoDetails;
    }
    
}


