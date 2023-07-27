

public class TutorialStep_GiveItemToPlayer : TutorialStep
{
    public TutorialStep_GiveItemToPlayer(string inItemId, SO_Item.EItemCategory inItemCategory)
    {
        ItemId = inItemId;
        ItemCategory = inItemCategory;
    }

    private string ItemId { get; set; }
    private SO_Item.EItemCategory ItemCategory { get; set; }
   
    public override void BeginStep()
    {
        GiveToPlayerItemInfo giveToPlayerItemInfo = new GiveToPlayerItemInfo()
        {
            itemId = ItemId, itemCategory = ItemCategory, amount = 1
        };
        GI.Inst.ListenerManager.GiveItemToPlayer(giveToPlayerItemInfo);
        IsCompleted = true;
    }

    public override void Execute(Tutorial tutorial)
    {
        if (IsCompleted)
        {
            tutorial.SetNextTutorialStep();
        }
    }

    public override void EndStep()
    {
        
    }
    
}
