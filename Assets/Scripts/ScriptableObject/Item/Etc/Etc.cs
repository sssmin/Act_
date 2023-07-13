using UnityEngine;



public enum ESkillMatId
{
    ActiveNormalMat,
    ActiveUltMat,
    PassiveMat
}

[CreateAssetMenu(fileName = "Etc_", menuName ="Data/Item/Etc")]
public class Etc : Item
{
    public Etc()
    {
        ItemCategory = EItemCategory.Etc;
        amount = 1;
    }
    [Header("Etc")]
    public EEtcCategory etcCategory;

    public void ItemCopy(Etc item)
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
    }

}