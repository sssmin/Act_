using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "CharacterStats")]
public class SO_PlayerBaseStats : ScriptableObject
{
    public Stats stats = new Stats();
}