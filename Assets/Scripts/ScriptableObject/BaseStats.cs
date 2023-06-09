using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "CharacterStats")]
public class BaseStats : ScriptableObjectType
{
    public Define.EBaseStatOwnerId ownerIdId;
    public int level;
    public int attack;
    public int defence;
    public int elemAttack;
    public int maxHp;
    public int criticalChancePer; // - %임 Value가 50이면 50%.
    public int evasionChancePer;
    public int skillCooltimeGrowthRate;
    public int moveSpeed;
}