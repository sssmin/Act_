using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonManager 
{
    public DungeonInfoDetail CurrentSelectDungeonInfoDetail { get; set; }
    private EDungeonType CurrentSelectDungeonType;
    private int ObjectiveCount { get; set; }
    private int CurrentKillCount { get; set; }
    public GameObject BossMapWall { get; set; }
    
    public void SelectDungeon(DungeonInfoDetail detail, EDungeonType type)
    {
        CurrentSelectDungeonInfoDetail = detail;
        CurrentSelectDungeonType = type;
        ObjectiveCount = 0;
        CurrentKillCount = 0;
    }

    public void AddObjectiveCount(int count)
    {
        ObjectiveCount += count;
    }

    public void IncreaseKillCount()
    {
        CurrentKillCount++;
        
        if (CurrentKillCount == ObjectiveCount)
        {
            foreach (DungeonRewardItemInfo rewardItemInfo in CurrentSelectDungeonInfoDetail.rewardItemIds)
            {
                float rand = Random.Range(0f, 100f);
                if (rand <= rewardItemInfo.dropChance)
                {
                    int giveAmount = rewardItemInfo.amount;
                    if (rewardItemInfo.amount > 1)
                    {
                        giveAmount = Random.Range(1, rewardItemInfo.amount + 1);
                    }
                    
                    GiveToPlayerItemInfo giveToPlayerItemInfo = new GiveToPlayerItemInfo()
                    {
                        amount = giveAmount, itemCategory = rewardItemInfo.itemCategory, itemId = rewardItemInfo.itemId
                    };
                    //todo 이 함수 말고 다른거 만들어서 UI 다른걸로 보이게
                    GI.Inst.ListenerManager.GiveItemToPlayer(giveToPlayerItemInfo);
                }
            }
           
            string type = Enum.GetName(typeof(EDungeonType), CurrentSelectDungeonType);
           
            GI.Inst.ResourceManager.DungeonLevelComplete(type, CurrentSelectDungeonInfoDetail.dungeonCategory, CurrentSelectDungeonInfoDetail.dungeonLevel);
            GI.Inst.SceneLoadManager.RequestLoadSceneAsync("Town", 2f);
        }
    }

    
}
