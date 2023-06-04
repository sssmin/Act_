using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections;
using Object = UnityEngine.Object;

public class ResourceManager : MonoBehaviour
{
    //prefabs
    private Dictionary<EPrefabId, Object> prefabs = new Dictionary<EPrefabId, Object>();
    //skill scriptable objects
    private Dictionary<Define.SkillId, Object> skills = new Dictionary<Define.SkillId, Object>();
    //base stats scriptable objects
    private Dictionary<Define.BaseStatOwnerId, Object> baseStats = new Dictionary<Define.BaseStatOwnerId, Object>();
    
    //data only
    //private Dictionary<string, Object> datas = new Dictionary<string, Object>();
    
    //sprites only.. but not use yet.
    //private Dictionary<string, Sprite> iconSprites = new Dictionary<string, Sprite>();

    
    

    public T Load<T>(EPrefabId id) where T : Object
    {
        if (prefabs.TryGetValue(id, out Object resource))
            return resource as T;
        
        return null;
    }
    
    public GameObject Instantiate(EPrefabId prefabName, Transform parent = null)
    {
        GameObject original = Load<GameObject>(prefabName);
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {prefabName}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            return GI.Inst.PoolManager.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }
    
    public GameObject Instantiate(EPrefabId prefabName, Vector3 parentPos, Quaternion parentRotation, Transform parent = null)
    {
        GameObject original = Load<GameObject>(prefabName);
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {prefabName}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            GameObject poolableGo = GI.Inst.PoolManager.Pop(original, parent).gameObject;
            poolableGo.transform.position = parentPos;
            poolableGo.transform.rotation = parentRotation;
            return poolableGo;
        }
        
        GameObject go = Object.Instantiate(original, parentPos, parentRotation, parent);
        go.name = original.name;
        
        return go;
    }
    
    // public ScriptableObject GetData(string dataName)
    // {
    //     if (datas.TryGetValue(dataName, out Object newObj))
    //     {
    //         return newObj as ScriptableObject;
    //     }
    //     return null;
    // }

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

    #region Addressable
    //Addressable 저장할 때 Data의 경우 AddressableName에 Data_???? 여기 물음표가 monsterId or itemId
    //dictionary에 저장할 때 key는 Data_????채로 저장됨. 불러올 때는 Id로만 불러온다.
    //함수 내에서 Data_를 조립해서 가져온다.
    

    private void LoadAsync<T>(string key, Action<string> callback = null) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            var asyncOperation = Addressables.LoadAssetAsync<T>(key);
            asyncOperation.Completed += (op) =>
            {
                PrefabId prefabId = (op.Result as GameObject).GetComponent<PrefabId>();
                if (prefabs.TryGetValue(prefabId.id, out Object resource))
                {
                    callback?.Invoke($"Failed to load. Has same key already. key : {key} ");
                    return;
                }
                prefabs.Add(prefabId.id, op.Result);
                
                callback?.Invoke("Load successfully.");
            };
        }
        else if (typeof(T) == typeof(ScriptableObject))
        {
            var asyncOperation = Addressables.LoadAssetAsync<T>(key);
            asyncOperation.Completed += (op) =>
            {
                ScriptableObjectType type = op.Result as ScriptableObjectType;
                
                switch (type.scriptableObjectType)
                {
                    case Define.ScriptableObjectType.Skill:
                    {
                        ActiveSkill skill = op.Result as ActiveSkill;
                        if (skills.TryGetValue(skill.skillId, out Object data))
                        {
                            callback?.Invoke($"Failed to load. Has same key already. key : {key} ");
                            return;
                        }
                        skills.Add(skill.skillId, op.Result);
                    }
                        break;
                    
                    case Define.ScriptableObjectType.Stat:
                    {
                        BaseStats stats = op.Result as BaseStats;
                        if (baseStats.TryGetValue(stats.ownerIdId, out Object data))
                        {
                            callback?.Invoke($"Failed to load. Has same key already. key : {key} ");
                            return;
                        }
                        baseStats.Add(stats.ownerIdId, op.Result);
                    }
                        break;
                }
                callback?.Invoke($"{key} : Load successfully.");
            };
            
            
            
            
        }
        // else if (typeof(T) == typeof(Sprite))
        // {
        //     if (iconSprites.TryGetValue(key, out Sprite value))
        //     {
        //         callback?.Invoke($"Failed to load. Has same key already. key : {key} ");
        //         return;
        //     }
        //     
        //     var asyncOperation = Addressables.LoadAssetAsync<IList<Sprite>>(key);
        //     asyncOperation.Completed += (aop) =>
        //     {
        //         foreach (Sprite spriteValue in aop.Result)
        //         {
        //             if (spriteValue.name.Substring(0, 4) == "Use_")
        //                 iconSprites.Add(spriteValue.name, spriteValue);
        //         }
        //         callback?.Invoke("Load successfully.");
        //     };
        // }
    }
    
    public void LoadAllAsync<T>(string label, Action<string, string, int, int> callback) where T : Object
    {
        //해당하는 라벨이 붙어있는 리소스를 모두 불러온다.
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        //모두 불러오는 것이 완료가 되면 호출할 함수를 콜백함수로 정의
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;
            
            //불러온 리소스 하나하나 돌면서 콜백 호출. multiple 스프라이트는 통채로 하나만 불러온 후 LoadAsync 함수에서 걸러냄 
            foreach (var result in op.Result)
            {
                //LoadAsync 함수 완료 후 실행될 콜백함수 정의
                //PrimaryKey => Addressable에 저장한 Addressable Name
                LoadAsync<T>(result.PrimaryKey, (successful) =>
                {
                    loadCount++;
                    callback?.Invoke(successful, result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    public ScriptableObject GetSkillDataCopy(Define.SkillId skillId)
    {
        if (skills.TryGetValue(skillId, out Object newObj))
        {
            ScriptableObject skillData = ScriptableObject.CreateInstance<ScriptableObject>();
            skillData = newObj as ScriptableObject;
            return skillData;
        }
        return null;
    }

    #endregion

    
    
}
