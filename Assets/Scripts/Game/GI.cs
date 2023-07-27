using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum EStartGameProgress
{
    None,
    NewGame,
    LoadGame,
    PlayingGame,
    GoToTitle
}



[Serializable]
class SaveResolution
{
    public SaveResolution(int inWidth, int inHeight, bool inIsFullScreen)
    {
        width = inWidth;
        height = inHeight;
        isFullScreen = inIsFullScreen;
    }
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public bool isFullScreen;
}

public class GI : MonoBehaviour
{
    public static GI Inst { get; private set; }
    public EStartGameProgress StartGameProgress { get; set; }
    
    [SerializeField] private List<Define.ELabel> labels = new List<Define.ELabel>();

    private PoolManager poolManager;
    private ListenerManager listenerManager;
    private DungeonManager dungeonManager;
    
    public Player Player { get; private set; }
    public CinemachineTarget CinemachineTarget { get; private set; } 
    
    public PoolManager PoolManager { get => Inst.poolManager; private set => poolManager = value; }
    public ListenerManager ListenerManager { get => Inst.listenerManager; private set => listenerManager = value; }
    public DungeonManager DungeonManager { get => Inst.dungeonManager; private set => dungeonManager = value; }

    public SkillManager PlayerSkillManager { get; private set; }
    public ResourceManager ResourceManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public CooltimeManager CooltimeManager { get; private set; }
    public SceneLoadManager SceneLoadManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public TutorialManager TutorialManager { get; private set; }
    
    private bool IsInitialized { get; set; }
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public bool IsFullscreen { get; set; }
    private Vector2 PlayerLoadedSpawnPos { get; set; }

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
        Inst.DungeonManager = new DungeonManager();

        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.sceneLoaded += SceneLoaded;
    }
    

    //맨처음 게임 씬에 들어갔을 때 호출될. 이게 newgame인지 로드인지
    void InitializeOnce()
    {
        Debug.Log("Initialize Once");
        Inst.ResourceManager = Generate<ResourceManager>("ResourceManager");
        Inst.ResourceManager.transform.SetParent(transform);

        Inst.CooltimeManager = Generate<CooltimeManager>("CooltimeManager");
        Inst.CooltimeManager.transform.SetParent(transform);
        
        Inst.SceneLoadManager = Generate<SceneLoadManager>("SceneLoadManager");
        Inst.SceneLoadManager.transform.SetParent(transform);

        Inst.SoundManager = Generate<SoundManager>("SoundManager");
        Inst.SoundManager.transform.SetParent(transform);

        Inst.TutorialManager = Generate<TutorialManager>("TutorialManager");
        Inst.TutorialManager.transform.SetParent(transform);

        //화면 크기 초기화
        LoadDisplayData();

        DownloadAdvance(() =>
        {
            //1. 플레이어 생성. 
            InitializePlayer();
            
            //2. 바인드 키 로드
            LoadBindKeyData();
            
            //3. 사운드 초기화
            Inst.SoundManager.AudioMixer = Inst.ResourceManager.AudioMixer;
            LoadSoundData();
            Inst.SoundManager.SFXPlay("Title", ESoundType.Background);

            //4. UI 초기화
            InitializeUI();
            
            //5. 타이틀 메뉴 생성
            Inst.ResourceManager.Instantiate("UI_TitleMainMenu");
        });
    }

    private void InitializePlayer()
    {
        if (Player)
        {
            Destroy(Player.gameObject);
        }
        GameObject go = Inst.ResourceManager.Instantiate("Player");
        Player = go.GetComponent<Player>();
        Player.GetComponent<InventoryManager>()?.BindAction();
        Player.gameObject.SetActive(false);
        PlayerSkillManager = go.GetComponent<SkillManager>();
        DontDestroyOnLoad(go);
    }

    private void InitializeUI()
    {
        if (Inst.UIManager)
        {
            Destroy(Inst.UIManager.gameObject);
        }
        Inst.UIManager = Generate<UIManager>("UIManager", true);
        Inst.UIManager.transform.parent = transform;
        Inst.UIManager.Init();
    }

    private void CameraInitialize()
    {
        GameObject cinemachineTargetGroup = Inst.ResourceManager.Instantiate("CinemachineTargetGroup");
        CinemachineTarget = cinemachineTargetGroup.GetComponent<CinemachineTarget>();
        CinemachineTarget.Init();
        
        GameObject mainUICameraGo = Inst.ResourceManager.Instantiate("MainUICamera");
        Camera mainUICamera = mainUICameraGo.GetComponent<Camera>();
        
        GameObject mainCameraGo = Inst.ResourceManager.Instantiate("MainCamera");
        MainCamera mainCamera = mainCameraGo.GetComponent<MainCamera>();
        mainCamera.Init(cinemachineTargetGroup.transform, mainUICamera);
    }

    private void DownloadAdvance(Action callback = null)
    {
        Debug.Log("DownloadAdvance");
        Inst.ResourceManager.DownloadAdvance(labels, callback);
    }
    
    private T Generate<T>(string objectName, bool isInitialize = false) where T : Component
    {
        GameObject go = null;
        if (isInitialize)
        {
            go = new GameObject(objectName);
            return go.AddComponent<T>();
        }
        
        go = GameObject.Find(objectName);
        if (go == null)
        {
            go = new GameObject(objectName);
            return go.AddComponent<T>();
        }
        
        return go.GetComponent<T>();
    }
    
    private void TitleInit()
    {
        //타이틀 화면 생성
        Inst.ResourceManager.Instantiate("UI_TitleMainMenu");

        //쿨타임 매니저도 새로
        Inst.CooltimeManager.InitCooltime();
        //플레이어 새로.
        InitializePlayer();
        LoadBindKeyData();
        InitializeUI();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //타이틀에서 한번만 초기화
        if (!IsInitialized)
        {
            IsInitialized = true;
            InitializeOnce();
        }
        else //씬 로드될때마다 위치, bgm 재생
        {
            switch (StartGameProgress)
            {
                case EStartGameProgress.NewGame:
                {
                    GameObject playerStartPoint = GameObject.Find("PlayerStartPoint");
                    Player.transform.position = playerStartPoint.transform.position;
                    StartGameProgress = EStartGameProgress.PlayingGame;
                    
                    CameraInitialize();
                    Inst.TutorialManager.ExecuteNewGameTutorial();
                }
                    break;
                case EStartGameProgress.PlayingGame:
                {
                    GameObject playerStartPoint = GameObject.Find("PlayerStartPoint");
                    Player.transform.position = playerStartPoint.transform.position;
                    StartGameProgress = EStartGameProgress.PlayingGame;
                    
                    CameraInitialize();
                    Inst.UIManager.VisibleAllMainUIComponent();

                    if (SceneManager.GetActiveScene().name == "Dungeon")
                    {
                        GameObject[] objects = GameObject.FindGameObjectsWithTag("SpawnLocation");
                        foreach (GameObject go in objects)
                        {
                            GameObject spawnerGo = Inst.ResourceManager.Instantiate("Spawner");
                            spawnerGo.transform.position = go.transform.position;
                            Spawner spawner = spawnerGo.GetComponent<Spawner>();
                            DungeonInfoDetail detail = DungeonManager.CurrentSelectDungeonInfoDetail;
                            spawner.SetMonsterInfo(detail.spawnMonsterIds, DungeonManager.CurrentSelectDungeonInfoDetail.monsterLevel, detail.dungeonCategory);
                        }
                    }
                    else if (SceneManager.GetActiveScene().name == "Boss")
                    {
                        GameObject[] objects = GameObject.FindGameObjectsWithTag("SpawnLocation");
                        foreach (GameObject go in objects)
                        {
                            GameObject spawnerGo = Inst.ResourceManager.Instantiate("Spawner");
                            spawnerGo.transform.position = go.transform.position;
                            Spawner spawner = spawnerGo.GetComponent<Spawner>();
                            DungeonInfoDetail detail = DungeonManager.CurrentSelectDungeonInfoDetail;
                            spawner.SetMonsterInfo(detail.spawnMonsterIds, DungeonManager.CurrentSelectDungeonInfoDetail.monsterLevel, detail.dungeonCategory);
                        }

                        GameObject bossWallGo = GameObject.FindGameObjectWithTag("BossWall");
                        bossWallGo.SetActive(false);
                        Inst.DungeonManager.BossMapWall = bossWallGo;
                    }
                }
                    break;
                case EStartGameProgress.LoadGame:
                {
                    Player.transform.position = PlayerLoadedSpawnPos;
                    StartGameProgress = EStartGameProgress.PlayingGame;
                    
                    CameraInitialize();
                    Inst.UIManager.VisibleAllMainUIComponent();
                }
                    break;
                case EStartGameProgress.GoToTitle:
                {
                    TitleInit();
                }
                    break;
            }
            Inst.SoundManager.SFXPlay(scene.name, ESoundType.Background);
        }
    }
    
    public void NewGame(EStartGameProgress startGameProgress)
    {
        Player.gameObject.SetActive(true);
        StartGameProgress = startGameProgress;
        Player.SetBaseStat();
        PlayerSkillManager.SetStartPassiveSkills();
        Inst.UIManager.RefreshInventoryUI();
        Inst.UIManager.RefreshGoldInvenCapacityUI();
        Inst.UIManager.InitTutorialUI();
    }
    
    public void SaveGame()
    {
        SaveInventoryData();
        SavePlayerData();
        SaveTutorialStatus();
        SaveDungeonInfo();
    }
    
    public string LoadGame(EStartGameProgress startGameProgress)
    {
        Player.gameObject.SetActive(true);
        StartGameProgress = startGameProgress;
        Player.SetBaseStat();
        
        string sceneName = LoadPlayerData();
        LoadInventoryData();
        LoadTutorialStatus();
        LoadDungeonInfo();
        
        return sceneName;
    }

    #region Save

    private void SaveInventoryData()
    {
        Serializable.S_InventoryInfo inventoryInfo = Player.InventoryManager.GetSerializeInventoryInfo();
        
        string path = Path.Combine(Application.persistentDataPath, "InventorySaveData.json");
        string jsonData = JsonUtility.ToJson(inventoryInfo);
        File.WriteAllText(path, jsonData);
    }

    private void SavePlayerData()
    {
        Serializable.S_PlayerInfo playerInfo = new Serializable.S_PlayerInfo(
            SceneManager.GetActiveScene().name, 
            Player.transform.position, 
            Player.StatManager.characterStats.currentHp.Value, 
            PlayerSkillManager.PassiveSkills,
            PlayerSkillManager.ActiveSkillLevels);
        
        string path = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        string jsonData = JsonUtility.ToJson(playerInfo);
        File.WriteAllText(path, jsonData);
    }
    
    private void SaveTutorialStatus()
    {
        TutorialStatusDictionary dict = Inst.TutorialManager.SerializeTutorialStatus();
        
        string path = Path.Combine(Application.persistentDataPath, "TutorialStatus.json");
        string jsonData = JsonUtility.ToJson(dict);
        File.WriteAllText(path, jsonData);
    }

    private void SaveDungeonInfo()
    {
        Serializable.S_DungeonInfo dungeonInfo = new Serializable.S_DungeonInfo(Inst.ResourceManager.DungeonInfos);
        
        string path = Path.Combine(Application.persistentDataPath, "DungeonInfoSaveData.json");
        string jsonData = JsonUtility.ToJson(dungeonInfo);
        File.WriteAllText(path, jsonData);
    }
    
    public void SaveSoundData()
    {
        Inst.SoundManager.GetAudioVolume(ESoundType.Master, out float value);
        PlayerPrefs.SetFloat("AudioMaster", value);
        
        Inst.SoundManager.GetAudioVolume(ESoundType.Background, out value);
        PlayerPrefs.SetFloat("AudioBackground", value);
        
        Inst.SoundManager.GetAudioVolume(ESoundType.Effect, out value);
        PlayerPrefs.SetFloat("AudioEffect", value);
    }
    
    public void SaveDisplayData()
    {
        string path = Path.Combine(Application.persistentDataPath, "DisplaySaveData.json");
        SaveResolution resolution = new SaveResolution(ScreenWidth, ScreenHeight, IsFullscreen);
        string resolutionJson = JsonUtility.ToJson(resolution);
        File.WriteAllText(path, resolutionJson);
    }
    
    public void SaveBindKeyData()
    {
        string path = Path.Combine(Application.persistentDataPath, "BindKeySaveData.json");
       
        foreach (InputAction inputAction in Player.PlayerController.PlayerControl)
        {
            foreach (var inputBinding in inputAction.bindings)
            {
                if (!string.IsNullOrEmpty(inputBinding.overridePath))
                {
                    inputAction.ChangeBinding(inputBinding).Erase();
                    InputBinding newBinding = new InputBinding(inputBinding.overridePath, inputBinding.action,
                        inputBinding.groups, inputBinding.processors, inputBinding.interactions, inputBinding.name);
                    inputAction.AddBinding(newBinding);
                }
            }
        }
        string bindingsJson = Player.PlayerController.PlayerControl.asset.ToJson();
        File.WriteAllText(path, bindingsJson);
    }
    

    #endregion //Save

    #region Load

    public void LoadInventoryData()
    {
        string path = Path.Combine(Application.persistentDataPath, "InventorySaveData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Serializable.S_InventoryInfo inventoryInfo = JsonUtility.FromJson<Serializable.S_InventoryInfo>(json);
            
            Player.InventoryManager.SetDeserializeInventoryInfo(inventoryInfo);
        }
    }
    
    private string LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Serializable.S_PlayerInfo playerInfo = JsonUtility.FromJson<Serializable.S_PlayerInfo>(json);
            PlayerLoadedSpawnPos = playerInfo.pos;
            PlayerStatManager playerStatManager = (PlayerStatManager)Player.StatManager;
            playerStatManager.InitCurrentHp(playerInfo.currentHp);
            PlayerSkillManager.SetDeserializeSkillInfo(playerInfo);
            return playerInfo.sceneName;
        }

        return "";
    }

    private void LoadTutorialStatus()
    {
        string path = Path.Combine(Application.persistentDataPath, "TutorialStatus.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            TutorialStatusDictionary dict = JsonUtility.FromJson<TutorialStatusDictionary>(json);
            Inst.TutorialManager.DeserializeTutorialStatus(dict);
        }
    }
    
    private void LoadDungeonInfo()
    {
        string path = Path.Combine(Application.persistentDataPath, "DungeonInfoSaveData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Serializable.S_DungeonInfo info = JsonUtility.FromJson<Serializable.S_DungeonInfo>(json);
            DeserializeDungeonInfo(info);
        }
    }
    
    private void LoadSoundData()
    {
        float value = PlayerPrefs.GetFloat("AudioMaster", 0f);
        Inst.SoundManager.SetAudioVolume(ESoundType.Master, value);
        
        value = PlayerPrefs.GetFloat("AudioBackground", 0f);
        Inst.SoundManager.SetAudioVolume(ESoundType.Background, value);
        
        value = PlayerPrefs.GetFloat("AudioEffect", 0f);
        Inst.SoundManager.SetAudioVolume(ESoundType.Effect, value);
    }
    
    private void LoadDisplayData()
    {
        string path = Path.Combine(Application.persistentDataPath, "DisplaySaveData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveResolution resolution = JsonUtility.FromJson<SaveResolution>(json);
            ScreenWidth = resolution.width;
            ScreenHeight = resolution.height;
            IsFullscreen = resolution.isFullScreen;
            Screen.SetResolution(ScreenWidth, ScreenHeight, IsFullscreen);
        }
    }

    public void LoadBindKeyData()
    {
        string path = Path.Combine(Application.persistentDataPath, "BindKeySaveData.json");
        if (File.Exists(path))
        {
            string bindingsJson = File.ReadAllText(path);
            InputActionAsset newAsset = InputActionAsset.FromJson(bindingsJson);
            foreach (var newActionMap in newAsset.actionMaps)
            {
                foreach (InputAction action in newActionMap.actions)
                {
                    InputAction existingAction = Player.PlayerController.PlayerControl.asset.FindAction(action.name);
                    if (existingAction != null)
                    {
                        existingAction.ChangeBinding(existingAction.bindings[0]).Erase();
                        InputBinding newBinding = new InputBinding(action.bindings[0].path, action.bindings[0].action,
                            action.bindings[0].groups, action.bindings[0].processors, action.bindings[0].interactions, action.bindings[0].name);
                        existingAction.AddBinding(newBinding);
                    }
                }
            }
        }
    }

    #endregion //Load
    
    public void ApplyResolution(int width, int height)
    {
        ScreenWidth = width;
        ScreenHeight = height;
        
        Screen.SetResolution(width, height, IsFullscreen);    
    }

    public void ApplyScreenMode(bool isFull)
    {
        IsFullscreen = isFull;
        Screen.SetResolution(ScreenWidth, ScreenHeight, isFull);
    }
    
    
    #region Serialize
    
    private void DeserializeDungeonInfo(Serializable.S_DungeonInfo info)
    {
        info.Deserialize();

        DungeonInfoDictionary dict = info.dungeonInfoDictionary;
        foreach (KeyValuePair<EDungeonType, Serializable.DungeonInfo_Lite> pair in dict)
        {
            Inst.ResourceManager.SetDungeonInfo(pair.Key, pair.Value);
        }
    }

    #endregion
}
