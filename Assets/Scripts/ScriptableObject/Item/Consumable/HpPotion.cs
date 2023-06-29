
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName ="Data/Item/Consumable/HpPotion")]
public class HpPotion : Consumable
{
    [HideInInspector] public Effect effect;
    
    public override void UseItem(StatManager casterStatManager)
    {
        effect = new Effect();
        Debug.Log("HPPotion 사용!");
    }
}
