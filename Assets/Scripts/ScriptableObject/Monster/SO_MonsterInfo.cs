using UnityEngine;

[CreateAssetMenu(fileName = "MonsterInfo_", menuName ="Data/MonsterInfo")]
public class SO_MonsterInfo : ScriptableObject
{
    public string monsterPrefabName;
    public string monsterName;
    public Stats stats = new Stats();
    public DropTable dropTable;
    

}
