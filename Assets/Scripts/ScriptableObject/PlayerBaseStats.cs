using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "CharacterStats")]
public class BaseStats : ScriptableObject
{
    public int level;
    public Stats stats = new Stats();
}