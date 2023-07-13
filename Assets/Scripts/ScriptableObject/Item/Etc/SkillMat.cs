using UnityEngine;

[CreateAssetMenu(fileName = "Etc_", menuName ="Data/Item/Etc/SkillMat")]
public class SkillMat : Etc
{
    [SerializeField] public ESkillMatId skillMatId;
    
    public void ItemCopy(SkillMat item)
    {
        itemId = item.itemId;
        ItemCategory = item.ItemCategory;
        itemName = item.itemName;
        itemDesc = item.itemDesc;
        itemIcon = item.itemIcon;
        //bIsCanStack = item.bIsCanStack;
        maxStackSize = item.maxStackSize;
        name = item.name;
        etcCategory = item.etcCategory;
        skillMatId = item.skillMatId;
    }
}
