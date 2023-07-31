using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName ="Data/Item/Consumable/HpPotion")]
public class SO_HpPotion : SO_Consumable
{
    public override void UseItem(StatManager casterStatManager)
    {
        PlayerStatManager playerStatManager = (PlayerStatManager)casterStatManager;
        playerStatManager.HealMaxHpPer(15f);
        GI.Inst.ResourceManager.Instantiate("HealingParticle", playerStatManager.gameObject.transform.position, Quaternion.identity, casterStatManager.transform);
    }
}
