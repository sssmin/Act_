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
    

}