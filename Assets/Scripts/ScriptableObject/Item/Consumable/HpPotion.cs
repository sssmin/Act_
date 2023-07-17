using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName ="Data/Item/Consumable/HpPotion")]
public class HpPotion : Consumable
{
    public override void UseItem(StatManager casterStatManager)
    {
        PlayerStatManager playerStatManager = (PlayerStatManager)casterStatManager;
        playerStatManager.HealMaxHpPer(15f);
    }
}
