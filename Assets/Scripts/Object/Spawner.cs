using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private int maxMonsterAmount = 6;
    int monsterLevel;
    List<string> monsterPrefabIds;
    

    public void SetMonsterInfo(List<string> monsterIds, int level, EDungeonCategory category)
    {
        if (category == EDungeonCategory.Normal)
            maxMonsterAmount = 6;
        else
            maxMonsterAmount = 1;
        
        monsterPrefabIds = monsterIds;
        monsterLevel = level;
        SpawnStart();
    }

    public void SpawnStart()
    {
        for (int i = 0; i < maxMonsterAmount; i++)
        {
            SpawnMonster();
        }
        GI.Inst.DungeonManager.AddObjectiveCount(maxMonsterAmount);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
    
    private void SpawnMonster()
    {
        if (monsterPrefabIds.Count <= 0) return;
        int rand = Random.Range(0, monsterPrefabIds.Count);
        GameObject go = GI.Inst.ResourceManager.Instantiate(monsterPrefabIds[rand], gameObject.transform.position, Quaternion.identity);
        GI.Inst.ResourceManager.GetMonsterInfoDataCopy(monsterPrefabIds[rand], Define.ELabel.MonsterInfo,
            monsterInfo =>
            {
                MonsterStatManager monsterStatManager = go.GetComponent<MonsterStatManager>();
                monsterStatManager.InitStat(monsterInfo.stats, monsterLevel);
                Monster monster = go.GetComponent<Monster>();
                monster.DropTable = monsterInfo.dropTable;
                monster.MonsterPrefabId = monsterPrefabIds[rand];
            });
    }
}
