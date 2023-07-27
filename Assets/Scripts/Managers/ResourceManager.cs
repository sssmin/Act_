using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager : MonoBehaviour
{
    public enum SaveType
    {
        Skills,
        Items,
        Prefab,
        PlayerBaseStat,
        AudioClip,
        AudioMixer,
        StatusSprites,
        WeaponEnhanceValueByLevel,
        DungeonInfo,
        ItemSprites
    }
   
    private Dictionary<string, SO_Skill> SkillData { get; } = new Dictionary<string, SO_Skill>();
    private Dictionary<string, SO_Item> ItemData { get; } = new Dictionary<string, SO_Item>();
    private Dictionary<string, GameObject> Prefabs { get; } = new Dictionary<string, GameObject>();
    private Dictionary<string, AudioClip> AudioClips { get; } = new Dictionary<string, AudioClip>();
    private Dictionary<string, Sprite> StatusSprites { get; } = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> ItemSprites { get; } = new Dictionary<string, Sprite>();
    public AudioMixer AudioMixer { get; set; }
    public SO_PlayerBaseStats PlayerBaseStats { get; private set; }
    public SO_WeaponEnhanceValueByLevel WeaponEnhanceValueByLevel { get; private set; }
    public Dictionary<string, SO_DungeonInfo> DungeonInfos { get; private set; } = new Dictionary<string, SO_DungeonInfo>();
    
    private int PrefabLoadCount { get; set; }
    
    public void DownloadAdvance(List<Define.ELabel> labels, Action callback = null)
    {
        int completedCount = 0;
        int totalCount = labels.Count;
        
        string label;
        
        for (int i = 0; i < totalCount; i++)
        {
            label = Enum.GetName(typeof(Define.ELabel), labels[i]);
            Addressables.DownloadDependenciesAsync(label).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    //Debug.Log("download completed");
                    completedCount++;
                }
                else
                {
                    Debug.Log("download failed.");
                }

                if (completedCount == totalCount)
                {
                    SaveData<ScriptableObject>(Define.ELabel.Skill, SaveType.Skills);
                    SaveData<ScriptableObject>(Define.ELabel.PlayerBaseStat, SaveType.PlayerBaseStat);
                    SaveData<ScriptableObject>(Define.ELabel.Item, SaveType.Items);
                    SaveData<ScriptableObject>(Define.ELabel.WeaponEnhanceValueByLevel, SaveType.WeaponEnhanceValueByLevel);
                    SaveData<ScriptableObject>(Define.ELabel.DungeonInfo, SaveType.DungeonInfo);
                    SaveData<AudioClip>(Define.ELabel.AudioClip, SaveType.AudioClip);
                    SaveData<AudioMixer>(Define.ELabel.AudioMixer, SaveType.AudioMixer);
                    SaveData<Sprite>(Define.ELabel.StatusSprite, SaveType.StatusSprites);
                    SaveData<Sprite>(Define.ELabel.ItemSprite, SaveType.ItemSprites);
                    SaveData<GameObject>(Define.ELabel.Prefab, SaveType.Prefab, callback);
                }
            };
        }
    }

    private void SaveData<T>(Define.ELabel inLabel, SaveType saveType, Action callback = null) where T : Object
    {
        string label = Enum.GetName(typeof(Define.ELabel), inLabel);
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int maxNum = op.Result.Count;
            PrefabLoadCount = 0;
            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, inLabel, maxNum, callback);
            }
        };
    }

    private void LoadAsync<T>(string key,Define.ELabel label, int maxNum, Action callback = null) where T : Object
    {
        switch (label)
        {
            case Define.ELabel.Skill:
                {
                    if (SkillData.ContainsKey(key))
                    {
                        Debug.Log($"Failed to load. Has same key already. key : {key} ");
                        return;
                    }
                    var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                    asyncOperation.Completed += (op) =>
                    {
                        SO_Skill original = op.Result as SO_Skill;
                        SO_Skill skillCopy = ScriptableObject.Instantiate(original);
                        SkillData.Add(key, skillCopy);
                    };
                }
                break;
            case Define.ELabel.Item:
                {
                    if (ItemData.ContainsKey(key))
                    {
                        Debug.Log($"Failed to load. Has same key already. key : {key} ");
                        return;
                    }
                    var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                    asyncOperation.Completed += (op) =>
                    {
                        SO_Item original = op.Result as SO_Item;
                        SO_Item itemCopy = ScriptableObject.Instantiate(original);
                        ItemData.Add(key, itemCopy);
                    };
                }
                break;
            case Define.ELabel.Prefab:
                {
                    if (Prefabs.ContainsKey(key))
                    {
                        Debug.Log($"Failed to load. Has same key already. key : {key} ");
                        return;
                    }
                    var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                    asyncOperation.Completed += (op) =>
                    {
                        Poolable poolable = (op.Result as GameObject).GetComponent<Poolable>();
                        if (poolable)
                        {
                            GI.Inst.PoolManager.CreatePoolAdvanced(poolable ,key);
                            PrefabLoadCount++;
                            if (PrefabLoadCount == maxNum)
                            {
                                callback?.Invoke();
                            }
                        }
                        else
                        {
                            Prefabs.Add(key, op.Result as GameObject);
                            PrefabLoadCount++;
                            if (PrefabLoadCount == maxNum)
                            {
                                callback?.Invoke();
                            }
                        }
                    };
                }
                break;
            case Define.ELabel.DungeonInfo:
            {
                if (DungeonInfos.ContainsKey(key))
                {
                    Debug.Log($"Failed to load. Has same key already. key : {key} ");
                    return;
                }
                var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                asyncOperation.Completed += (op) =>
                {
                    SO_DungeonInfo original = op.Result as SO_DungeonInfo;
                    SO_DungeonInfo dungeonInfoCopy = ScriptableObject.Instantiate(original);
                    DungeonInfos.Add(key, dungeonInfoCopy);
                };
            }
                break;
            case Define.ELabel.PlayerBaseStat:
            {
                var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                asyncOperation.Completed += (op) =>
                {
                    SO_PlayerBaseStats original = op.Result as SO_PlayerBaseStats;
                    SO_PlayerBaseStats baseStatsCopy = ScriptableObject.Instantiate(original);
                    PlayerBaseStats = baseStatsCopy;
                };
            }
                break;
            case Define.ELabel.AudioClip:
            {
                if (AudioClips.ContainsKey(key))
                {
                    Debug.Log($"Failed to load. Has same key already. key : {key} ");
                    return;
                }
                var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                asyncOperation.Completed += (op) =>
                {
                    AudioClips.Add(key, op.Result as AudioClip);
                };
            }
                break;
            case Define.ELabel.AudioMixer:
            {
                var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                asyncOperation.Completed += (op) =>
                {
                    AudioMixer = op.Result as AudioMixer;
                };
            }
                break;
            case Define.ELabel.StatusSprite:
            {
                if (StatusSprites.ContainsKey(key))
                {
                    Debug.Log($"Failed to load. Has same key already. key : {key} ");
                    return;
                }
                
                var asyncOperation = Addressables.LoadAssetAsync<IList<Sprite>>(key);
                asyncOperation.Completed += (aop) =>
                {
                    foreach (Sprite spriteValue in aop.Result)
                    {
                        StatusSprites.Add(spriteValue.name, spriteValue);
                    }
                };
            }
                break;
            case Define.ELabel.ItemSprite:
            {
                if (ItemSprites.ContainsKey(key))
                {
                    Debug.Log($"Failed to load. Has same key already. key : {key} ");
                    return;
                }
                
                var asyncOperation = Addressables.LoadAssetAsync<IList<Sprite>>(key);
                asyncOperation.Completed += (aop) =>
                {
                    foreach (Sprite spriteValue in aop.Result)
                    {
                        ItemSprites.Add(spriteValue.name, spriteValue);
                    }
                };
            }
                break;
            case Define.ELabel.WeaponEnhanceValueByLevel:
            {
                var asyncOperation = Addressables.LoadAssetAsync<T>(key);
                asyncOperation.Completed += (op) =>
                {
                    SO_WeaponEnhanceValueByLevel original = op.Result as SO_WeaponEnhanceValueByLevel;
                    SO_WeaponEnhanceValueByLevel copy = ScriptableObject.Instantiate(original);
                    WeaponEnhanceValueByLevel = copy;
                };
            }
                break;
        }
    }
    
    public GameObject Instantiate(string prefabName, Transform parent = null)
    {
        GameObject go = null;
        if (GI.Inst.PoolManager.IsPoolable(prefabName))
        {
            go = GI.Inst.PoolManager.Pop(prefabName, parent).gameObject;
            go.name = prefabName;
        }
        else
        {
            if (Prefabs.ContainsKey(prefabName))
            {
                go = Instantiate(Prefabs[prefabName], parent);
                go.name = prefabName;
            }
            else
            {
                Debug.Log($"Instantiate 실패. {prefabName}프리팹이 저장되어있지 않음");
            }
        }
        return go;
    }
    
    
    public GameObject Instantiate(string prefabName, Vector3 parentPos, Quaternion parentRotation ,Transform parent = null)
    {
        GameObject go = null;
        if (GI.Inst.PoolManager.IsPoolable(prefabName))
        {
            go = GI.Inst.PoolManager.Pop(prefabName, parent).gameObject;
            go.transform.position = parentPos;
            go.transform.rotation = parentRotation;
            go.name = prefabName;
        }
        else
        {
            if (Prefabs.ContainsKey(prefabName))
            {
                go = Object.Instantiate(Prefabs[prefabName], parentPos, parentRotation, parent);
                go.name = prefabName;
            }
            else
            {
                Debug.Log($"Instantiate 실패. {prefabName}프리팹이 저장되어있지 않음");
            }
        }
        return go;
    }

    public void Destroy(GameObject go, float time = 0f)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            StartCoroutine(CoRestoreForSeconds(poolable, time));
            return;
        }

        Object.Destroy(go, time);
    }

    IEnumerator CoRestoreForSeconds(Poolable poolable, float time)
    {
        yield return new WaitForSeconds(time);
        GI.Inst.PoolManager.Restore(poolable);
    }
    
    public SO_ActiveSkill GetActiveSkillData(Define.ESkillId skillId)
    {
        string key = Enum.GetName(typeof(Define.ESkillId), skillId);
        if (SkillData.ContainsKey(key))
            return (SO_ActiveSkill)SkillData[key];
        
        return null;
    }
    
    public SO_PassiveSkill GetPassiveSkillData(Define.ESkillId skillId)
    {
        string key = Enum.GetName(typeof(Define.ESkillId), skillId);
        if (SkillData.ContainsKey(key))
            return (SO_PassiveSkill)SkillData[key];
        
        return null;
    }
    
    public SO_Item GetItemDataCopy(string itemId)
    {
        if (ItemData.ContainsKey(itemId))
            return ScriptableObject.Instantiate(ItemData[itemId]);
        
        return null;
    }

    public SO_Item GetItemData(string itemId)
    {
        if (ItemData.ContainsKey(itemId))
            return ItemData[itemId];
        
        return null;
    }

    public SO_DungeonInfo GetDungeonInfo(EDungeonType dungeonType)
    {
        string key = Enum.GetName(typeof(EDungeonType), dungeonType);
        if (DungeonInfos.ContainsKey(key))
            return DungeonInfos[key];
        return null;
    }
    
    public void GetMonsterInfoDataCopy(string monsterPrefabName, Define.ELabel label ,Action<SO_MonsterInfo> callback)
    {
        string key = Enum.GetName(typeof(Define.ELabel), label);
        var opHandle = Addressables.LoadResourceLocationsAsync(key, typeof(ScriptableObject));

        opHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var result in op.Result)
                {
                    var asyncOperation = Addressables.LoadAssetAsync<ScriptableObject>(result.PrimaryKey);
                    asyncOperation.Completed += (operationHandle) =>
                    {
                        SO_MonsterInfo monsterInfoCopy = ScriptableObject.Instantiate(operationHandle.Result as SO_MonsterInfo);
                   
                        if (monsterInfoCopy.monsterPrefabName == monsterPrefabName)
                        {
                            callback?.Invoke(monsterInfoCopy);
                        }
                    };
                }
            }
        };
    }

    public AudioClip GetAudioClip(string name)
    {
        if (AudioClips.ContainsKey(name))
            return AudioClips[name];
        return null;
    }

    public Sprite GetStatusSprite(string name)
    {
        if (StatusSprites.ContainsKey(name))
            return StatusSprites[name];
        return null;
    }

    public Sprite GetItemSprite(string name)
    {
        if (ItemSprites.ContainsKey(name))
            return ItemSprites[name];
        return null;
    }
    
    public void CreateItemCraft(Define.ELabel label ,Action<ItemCraft> callback)
    {
        string key = Enum.GetName(typeof(Define.ELabel), label);
        var opHandle = Addressables.LoadResourceLocationsAsync(key, typeof(ScriptableObject));

        opHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var result in op.Result)
                {
                    var asyncOperation = Addressables.LoadAssetAsync<ScriptableObject>(result.PrimaryKey);
                    asyncOperation.Completed += (operationHandle) =>
                    {
                        ItemCraft itemCraft = ScriptableObject.Instantiate(operationHandle.Result as ItemCraft);

                        callback?.Invoke(itemCraft);
                        //Addressables.Release(operationHandle);
                    };
                }
            }
            //Addressables.Release(op);
        };
    }
    
    public void SetDungeonInfo(EDungeonType type, Serializable.DungeonInfo_Lite dungeonInfoLite)
    {
        string key = Enum.GetName(typeof(EDungeonType), type);
        if (DungeonInfos.ContainsKey(key))
        {
            DungeonInfos[key].CopyFromLite(dungeonInfoLite);
        }
    }

    public bool IsPrevDungeonCompleted(EDungeonType type)
    {
        if ((int)type == 0) return true;
        EDungeonType prevDungeonType = (EDungeonType)((int)type - 1);
        string key = Enum.GetName(typeof(EDungeonType), prevDungeonType);
        if (DungeonInfos.ContainsKey(key))
        {
            return DungeonInfos[key].isDungeonCompleted;
        }

        return false;
    }

    public void DungeonLevelComplete(string key, EDungeonCategory category, int dungeonLevel)
    {
        if (DungeonInfos.ContainsKey(key))
        {
            if (category == EDungeonCategory.Normal)
            {
                DungeonInfoDetail detail = DungeonInfos[key].dungeonInfoDetails.Find(detail =>
                    (detail.dungeonCategory == category) && (detail.dungeonLevel == dungeonLevel));
                detail.isLevelCompleted = true;
            }
            else if (category == EDungeonCategory.Boss)
            {
                DungeonInfoDetail detail = DungeonInfos[key].dungeonInfoDetails.Find(detail =>
                    (detail.dungeonCategory == category) && (detail.dungeonLevel == dungeonLevel));
                detail.isLevelCompleted = true;
                DungeonInfos[key].isDungeonCompleted = true;
            }
        }
    }


}
