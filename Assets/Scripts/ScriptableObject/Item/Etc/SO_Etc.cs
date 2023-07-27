using UnityEngine;


public enum ESkillMatId
{
    ActiveNormalMat,
    ActiveUltMat,
    PassiveMat
}

[CreateAssetMenu(fileName = "Etc_", menuName ="Data/Item/Etc")]
public class SO_Etc : SO_Item
{
    public SO_Etc()
    {
        ItemCategory = EItemCategory.Etc;
        amount = 1;
    }
    [Header("Etc")]
    public EEtcCategory etcCategory;
    

}