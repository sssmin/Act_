using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DropInfo
{
    [SerializeField] public string itemId;
    [SerializeField] public int maxDropAmount;
    [SerializeField] public float dropChance;
}

public class GiveToPlayerItemInfo
{
    public Item item;
    public int amount;
}

[Serializable]
public class DropTable
{
    [SerializeField] public List<DropInfo> itemInfos = new List<DropInfo>();

    public void DropItem()
    {
        int playerInstId = GI.Inst.ListenerManager.GetPlayerInstId();
        List<GiveToPlayerItemInfo> giveToPlayerItemInfos = new List<GiveToPlayerItemInfo>();
        foreach (DropInfo itemInfo in itemInfos)
        {
            float rand = Random.Range(0f, 100f);
            if (rand <= itemInfo.dropChance)
            {
                int randAmount = Random.Range(1, itemInfo.maxDropAmount);

                Item dropItem = GI.Inst.ResourceManager.GetItemData(itemInfo.itemId);
                GiveToPlayerItemInfo giveToPlayerItemInfo = new GiveToPlayerItemInfo()
                {
                    item = dropItem, amount = randAmount
                };
                
                giveToPlayerItemInfos.Add(giveToPlayerItemInfo);
               
            }
        }
        GI.Inst.ListenerManager.GiveItemsToPlayer(playerInstId, giveToPlayerItemInfos);
    }

}
