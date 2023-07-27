using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private class Pool
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
            
            poolable.transform.SetParent(Head);
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

            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }
    
    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    private Transform root;
    
    
    public void Init()
    {
        if (root == null)
        {
            root = new GameObject().transform;
            root.name = "Pool_Root";
            Object.DontDestroyOnLoad(root);
        }
    }

    public bool IsPoolable(string key)
    {
        if (pools.ContainsKey(key))
            return true;
        return false;
    }

    private void CreatePool(GameObject origin, string prefabId, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(origin, count);
        pool.Head.parent = root;

        pools.Add(prefabId, pool);
    }

    public void Restore(Poolable poolable)
    {
        string prefabId = poolable.GetComponent<PrefabName>()?.Value; 
        
        if (pools.ContainsKey(prefabId) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        
        pools[prefabId].Push(poolable);
    }

    public Poolable Pop(GameObject origin, string prefabId , Transform parent = null)
    {
        if (pools.ContainsKey(prefabId) == false)
        {
            CreatePool(origin, prefabId);
        }
        return pools[prefabId].Pop(parent);
    }
    
    public Poolable Pop(string prefabId, Transform parent = null)
    {
        return pools[prefabId].Pop(parent);
    }

    public void Clear()
    {
        foreach (Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }
        pools.Clear();
    }
    

    public void CreatePoolAdvanced(Poolable poolable, string prefabId)
    {
        if (poolable)
        {
            CreatePool(poolable.gameObject,  prefabId, poolable.poolingNum);
        }
    }
    
}
