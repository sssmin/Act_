using System;
using UnityEngine;

public class GI : MonoBehaviour
{
    [SerializeField] GameObject playerStartPoint;
    [SerializeField] CinemachineTarget cinemachineTarget;
    public static GI Inst { get; private set; }

    private PoolManager poolManager;
    private ListenerManager listenerManager;
    
    // public PlayerManager PlayerManager { get; private set; }
    public ResourceManager ResourceManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }

    public PoolManager PoolManager 
    { 
        get { return Inst.poolManager; }
        private set => poolManager = value;
    }

    public ListenerManager ListenerManager
    {
        get { return Inst.listenerManager; }
        private set => listenerManager = value;
    }

    public Player Player { get; private set; }
    public SkillManager PlayerSkillManager { get; private set; }
     

    void Awake()
    {
        if (Inst != null)
        {
             Destroy(gameObject);
             return;
        }
         
        Inst = this;
        DontDestroyOnLoad(this);
        Inst.PoolManager = new PoolManager();
        Inst.ListenerManager = new ListenerManager();
    }

    private int initCompleteNum;
    private int currentInitCount;
    

    void Start()
    {
        Init();
    }
    
    void Init()
    {

        Inst.ResourceManager = Generate<ResourceManager>("ResourceManager");
        Inst.ResourceManager.transform.parent = transform;
       
        
        PrefabLoadAsync();
        
        //얘는 일단 보류. 아래 ScriptableObject에 icon을 같이 저장을 했기 때문에.
        // Inst.ResourceManager.LoadAllAsync<Sprite>("Sprites" , (successful, key, count, totalCount) =>
        // {
        //     Debug.Log($"{successful} - {key} : {count}/{totalCount}");
        //
        //     if (count == totalCount)
        //     {
        //         
        //     }
        // });
        
        StatLoadAsync();
        SkillDataLoadAsync();
        // Inst.PlayerManager = Generate<PlayerManager>("PlayerManager");
        // Inst.PlayerManager.transform.parent = transform;
    }

    void Synchronization()
    {
        if (++currentInitCount == initCompleteNum)
        {
            //todo 여기는 위에서 비동기로 Load한게 모두 끝난 부분
            //todo 스탯 초기화
            
            cinemachineTarget.Init();
            PlayerSkillManager.InitSkills();
            
        }

       
    }
    
    private void PrefabLoadAsync()
    {
        initCompleteNum++;
        Inst.ResourceManager.LoadAllAsync<GameObject>("Prefab", (successful, key, count, totalCount) =>
        {
            //Debug.Log($"{successful} - {key} : {count}/{totalCount}");
            //모두 로드 완료
            if (count == totalCount)
            {
                //Pooling 할 오브젝트 초기화
                Inst.PoolManager.Init();

                Inst.InventoryManager = Generate<InventoryManager>("InventoryManager");
                Inst.InventoryManager.transform.parent = transform;

                GameObject go = Inst.ResourceManager.Instantiate(EPrefabId.Player, playerStartPoint.transform.position,
                    Quaternion.identity);
                Player = go.GetComponent<Player>();
                PlayerSkillManager = go.GetComponent<SkillManager>();
                
                Synchronization();
            }
        });
    }

   
    private void StatLoadAsync()
    {
        initCompleteNum++;
        Inst.ResourceManager.LoadAllAsync<ScriptableObject>("Stat", (successful, key, count, totalCount) =>
        {
            //Debug.Log($"{successful} - {key} : {count}/{totalCount}");
            if (count == totalCount)
            {
                //Inst.ListenerManager.OnDataLoadCompleted();
                Synchronization();
            }
        });
    }
    
    private void SkillDataLoadAsync()
    {
        initCompleteNum++;
        Inst.ResourceManager.LoadAllAsync<ScriptableObject>("Skill", (successful, key, count, totalCount) =>
        {
            //Debug.Log($"{successful} - {key} : {count}/{totalCount}");
            if (count == totalCount)
            {
               
                //Inst.ListenerManager.OnDataLoadCompleted();;
                Synchronization();
            }
        });
    }

    T Generate<T>(string objectName) where T : UnityEngine.Component
    {
        GameObject go = GameObject.Find(objectName);
        if (go == null)
        {
            go = new GameObject();
            go.name = objectName;
            return go.AddComponent<T>();
        }
        
        return go.GetComponent<T>();
    }
    
}
