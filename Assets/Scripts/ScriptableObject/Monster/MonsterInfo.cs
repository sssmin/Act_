using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MonsterInfo_", menuName ="Data/MonsterInfo")]
public class MonsterInfo : ScriptableObject
{
    public string monsterPrefabName;
    public string monsterName;
    public Stats stats = new Stats();
    public DropTable dropTable;
    

}
