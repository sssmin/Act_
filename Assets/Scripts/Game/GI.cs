using System;
using System.Collections.Generic;
using UnityEngine;

public class GI : MonoBehaviour
{
    public static GI Inst { get; private set; }
    
    [SerializeField] private List<Define.ELabel> labels = new List<Define.ELabel>();
    [SerializeField] public Camera uiCamrea;
    public Player Player { get; private set; }
    public SkillManager PlayerSkillManager { get; private set; }
    [SerializeField] GameObject playerStartPoint;
    [SerializeField] public CinemachineTarget cinemachineTarget;
    private int initCompleteNum;
    private int currentInitCount;
    
    
    private PoolManager poolManager;
    private ListenerManager listenerManager;
    public PoolManager PoolManager { get => Inst.poolManager; private set => poolManager = value; }
    public ListenerManager ListenerManager { get => Inst.listenerManager; private set => listenerManager = value; }

    public ResourceManager ResourceManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public CooltimeManager CooltimeManager { get; private set; }

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
        Inst.PoolManager.Init();
        Inst.ListenerManager = new ListenerManager();
    }
    

    void Start()
    {
        Init();
    }
    
    void Init()
    {
        Inst.ResourceManager = Generate<ResourceManager>("ResourceManager");
        Inst.ResourceManager.transform.SetParent(transform);

        Inst.CooltimeManager = Generate<CooltimeManager>("CooltimeManager");
        Inst.CooltimeManager.transform.SetParent(transform);
        
        DownloadAdvance(Synchronization);
    }

    void Synchronization()
    {
        Debug.Log("Synchronization");
        //1. 플레이어 생성. 
        GameObject go = Inst.ResourceManager.Instantiate("Player", playerStartPoint.transform.position,
                Quaternion.identity);
        Player = go.GetComponent<Player>();
        Player.GetComponent<InventoryManager>()?.BindAction();
        //2. 시작 베이스 스탯 초기화 todo OR 저장했던 스탯 데이터로 초기화
        Player.SetBaseStat();
        //3. UI 초기화(생성만)
        Inst.UIManager = Generate<UIManager>("UIManager");
        Inst.UIManager.transform.parent = transform;
        Inst.UIManager.Init();
        
        //4. 플레이어 시작 아이템 초기화, 스탯 수정, UI 갱신 todo OR 저장했던 아이템 데이터로 초기화
        InventoryManager inventory = go.GetComponent<InventoryManager>();
        inventory.SetStartItem();
        
        PlayerSkillManager = go.GetComponent<SkillManager>();
        //todo 아마도 스킬 레벨 초기화
        PlayerSkillManager.InitSkills();
        
        
        
        cinemachineTarget.Init();
        
       
        
        
        // 마지막에 UI 세팅
        Inst.UIManager.RefreshInventoryUI();
        Inst.UIManager.RefreshGoldInvenCapacityUI();
    }
    

    private void DownloadAdvance(Action callback)
    {
        Debug.Log("DownloadAdvance");
        Inst.ResourceManager.DownloadAdvance(labels, callback);
    }
    
    T Generate<T>(string objectName) where T : Component
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
