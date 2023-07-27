using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DropInfo
{
    [SerializeField] public string itemId;
    [SerializeField] public SO_Item.EItemCategory itemCategory;
    [SerializeField] public int maxDropAmount;
    [SerializeField] public float dropChance;
}

public class GiveToPlayerItemInfo
{
    public string itemId;
    public SO_Item.EItemCategory itemCategory;
    public int amount;
}

[Serializable]
public class DropTable
{
    [SerializeField] public List<DropInfo> itemInfos = new List<DropInfo>();
    [SerializeField] public int gold;
    
    public void DropItem()
    {
        List<GiveToPlayerItemInfo> giveToPlayerItemInfos = new List<GiveToPlayerItemInfo>();
        foreach (DropInfo dropInfo in itemInfos)
        {
            float rand = Random.Range(0f, 100f);
            if (rand <= dropInfo.dropChance)
            {
                int randAmount = Random.Range(1, dropInfo.maxDropAmount);

                GiveToPlayerItemInfo giveToPlayerItemInfo = new GiveToPlayerItemInfo()
                {
                    itemId = dropInfo.itemId, amount = randAmount, itemCategory = dropInfo.itemCategory
                };
                
                giveToPlayerItemInfos.Add(giveToPlayerItemInfo);
            }
        }

        float goldRand = Random.Range(0f, 100f);
        if (goldRand <= 40f)
        {
            int goldWeight = Mathf.FloorToInt(gold * 0.1f);
            goldWeight = Random.Range(-goldWeight, goldWeight + 1);
            GI.Inst.ListenerManager.GiveGoldToPlayer(gold + goldWeight);
            GI.Inst.ListenerManager.GiveItemsToPlayer(giveToPlayerItemInfos);
        }
        
    }

}
