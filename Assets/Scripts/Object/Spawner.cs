using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] int maxMonsterAmount;
    [SerializeField] int monsterLevel;
    [SerializeField] List<string> monsterPrefabIds;

    private BoxCollider2D boxCollder;

    private bool IsActivated { get; set; }

    private void Awake()
    {
        boxCollder = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (!boxCollder)
        {
            //바로 스폰
            SpawnMonster();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!IsActivated)
            {
                StartCoroutine(CoSpawn());
                IsActivated = true;
            }
        }
    }

    IEnumerator CoSpawn()
    {
        for (int i = 0; i < maxMonsterAmount; i++)
        {
            SpawnMonster();
            yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        }
        GI.Inst.ResourceManager.Destroy(gameObject);
    }

    void SpawnMonster()
    {
        if (monsterPrefabIds.Count <= 0) return;
        int rand = Random.Range(0, monsterPrefabIds.Count);
        GameObject go = GI.Inst.ResourceManager.Instantiate(monsterPrefabIds[rand], gameObject.transform.position, Quaternion.identity);
        GI.Inst.ResourceManager.GetMonsterInfoDataCopy(monsterPrefabIds[rand], Define.ELabel.MonsterInfo,
            monsterInfo =>
            {
                StatManager monsterStatManager = go.GetComponent<StatManager>();
                monsterStatManager.InitStat(monsterInfo.stats);
                Monster monster = go.GetComponent<Monster>();
                monster.DropTable = monsterInfo.dropTable;
                monster.MonsterPrefabId = monsterPrefabIds[rand];
            });
    }
}
