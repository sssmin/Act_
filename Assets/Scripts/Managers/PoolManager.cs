using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Head { get; set; }

        private Stack<Poolable> poolables = new Stack<Poolable>();

        public void Init(GameObject origin, int count = 5)
        {
            Original = origin;
            Head = new GameObject().transform;
            Head.name = $"{origin.name}_Head";

            for (int i = 0; i < count; i++)
                Push(CreateCopyObject());
        }

        Poolable CreateCopyObject()
        {
            GameObject go = Object.Instantiate(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;
            
            
            poolable.transform.parent = Head;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;
            
            poolables.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            
            if (poolables.Count > 0)
                poolable = poolables.Pop();
            else 
                poolable = CreateCopyObject();

            poolable.gameObject.SetActive(true);

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    
    private Dictionary<EPrefabId, Pool> pools = new Dictionary<EPrefabId, Pool>();
    private Transform root;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject().transform;
            root.name = "Pool_Root";
            Object.DontDestroyOnLoad(root);
        }
        
        CreatePoolAdvanced();
    }

    public void CreatePool(GameObject origin, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(origin, count);
        pool.Head.parent = root;

        PrefabId prefabId = origin.GetComponent<PrefabId>();
        pools.Add(prefabId.id, pool);
    }

    public void Restore(Poolable poolable)
    {
        PrefabId prefabId = poolable.GetComponent<PrefabId>();
        if (pools.ContainsKey(prefabId.id) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        
        pools[prefabId.id].Push(poolable);
    }

    public Poolable Pop(GameObject origin, Transform parent = null)
    {
        PrefabId prefabId = origin.GetComponent<PrefabId>();
        if (pools.ContainsKey(prefabId.id) == false)
        {
            CreatePool(origin);
        }
        return pools[prefabId.id].Pop(parent);
    }

    public GameObject GetOriginal(EPrefabId id)
    {
        if (pools.ContainsKey(id) == false) return null;
        return pools[id].Original;
    }

    public void Clear()
    {
        foreach (Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }
        pools.Clear();
    }

    public void CreatePoolAdvanced(EPrefabId name, int count)
    {
        if (pools.ContainsKey(name) == false)
        {
            GameObject origin = GI.Inst.ResourceManager.Load<GameObject>(name);
            CreatePool(origin, count);
        }
    }

    public void CreatePoolAdvanced()
    {
        foreach (KeyValuePair<EPrefabId, Object> pair in GI.Inst.ResourceManager.Prefabs)
        {
            GameObject prefab = (GameObject)pair.Value;
            Poolable poolable = prefab.GetComponent<Poolable>();
            if (poolable)
            {
                CreatePool(prefab, poolable.poolingNum);
            }
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
