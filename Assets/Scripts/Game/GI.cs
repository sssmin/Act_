using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum EScene
{
    Title,
    Tutorial,
    Town,
    Dungeon,
    Boss
}

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
  
    public Player Player { get; private set; }
    public SkillManager PlayerSkillManager { get; private set; }

    public CinemachineTarget CinemachineTarget { get; private set; } 
    private PoolManager poolManager;
    private ListenerManager listenerManager;
    public PoolManager PoolManager { get => Inst.poolManager; private set => poolManager = value; }
    public ListenerManager ListenerManager { get => Inst.listenerManager; private set => listenerManager = value; }

    public ResourceManager ResourceManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public CooltimeManager CooltimeManager { get; private set; }
    public SceneLoadManager SceneLoadManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    private bool IsInitialized { get; set; }
    
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public bool IsFullscreen { get; set; }

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

    public void InitializePlayer()
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

    public void InitializeUI()
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
    
    T Generate<T>(string objectName, bool isInitialize = false) where T : Component
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
    
    void TitleInit()
    {
        //타이틀 화면 생성
        Inst.ResourceManager.Instantiate("UI_TitleMainMenu");
        
        //리소스 저장정보 새로 초기화해야할듯함.
        List<Define.ELabel> labels = new List<Define.ELabel>()
        {
            Define.ELabel.Skill, Define.ELabel.Item 
        };
        Inst.ResourceManager.InitData(labels);
        
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
                }
                    break;
                case EStartGameProgress.PlayingGame:
                {
                    GameObject playerStartPoint = GameObject.Find("PlayerStartPoint");
                    Player.transform.position = playerStartPoint.transform.position;
                    StartGameProgress = EStartGameProgress.PlayingGame;
                    CameraInitialize();
                }
                    break;
                case EStartGameProgress.LoadGame:
                {
                    PlayerInfo playerInfo = LoadPlayerData();
                
                    Player.transform.position = playerInfo.pos;
                    StartGameProgress = EStartGameProgress.PlayingGame;
                    CameraInitialize();
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
        InventoryManager inventory = Player.GetComponent<InventoryManager>();
        inventory.SetStartItem();
        PlayerSkillManager.SetStartPassiveSkills();
    }
    //todo 이 둘 함수명 변경해야함. 이거 AsyncLoader에서 호출하고 있고 얘네는 타이틀에서 한번만 호출됨
    public string LoadGame(EStartGameProgress startGameProgress)
    {
        Player.gameObject.SetActive(true);
        StartGameProgress = startGameProgress;
        Player.SetBaseStat();
        PlayerInfo playerInfo = LoadPlayerData();
        PlayerStatManager playerStatManager = (PlayerStatManager)Player.StatManager;
        playerStatManager.InitCurrentHp(playerInfo.currentHp);
        
        LoadInventoryData();
        PlayerSkillManager.SetDeserializeSkillInfo(playerInfo);
        
        return playerInfo.sceneName;
    }

    public void SaveGameData()
    {
        InventoryInfo inventoryInfo = Player.InventoryManager.GetSerializeInventoryInfo();
        
        string path = Path.Combine(Application.persistentDataPath, "InventorySaveData.json");
        string jsonData = JsonUtility.ToJson(inventoryInfo);
        File.WriteAllText(path, jsonData);
        
        PlayerInfo playerInfo = new PlayerInfo(
            SceneManager.GetActiveScene().name, 
            Player.transform.position, 
            Player.StatManager.stats.currentHp.Value, 
            PlayerSkillManager.PassiveSkills,
            PlayerSkillManager.ActiveSkillLevels);
        path = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        jsonData = JsonUtility.ToJson(playerInfo);
        File.WriteAllText(path, jsonData);
    }
    
    public void LoadInventoryData()
    {
        string path = Path.Combine(Application.persistentDataPath, "InventorySaveData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryInfo inventoryInfo = JsonUtility.FromJson<InventoryInfo>(json);
            
            Player.InventoryManager.SetDeserializeInventoryInfo(inventoryInfo);
        }
    }
    
    public PlayerInfo LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<PlayerInfo>(json);
        }
        return null;
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
    
    public void LoadSoundData()
    {
        float value = PlayerPrefs.GetFloat("AudioMaster", 0f);
        Inst.SoundManager.SetAudioVolume(ESoundType.Master, value);
        
        value = PlayerPrefs.GetFloat("AudioBackground", 0f);
        Inst.SoundManager.SetAudioVolume(ESoundType.Background, value);
        
        value = PlayerPrefs.GetFloat("AudioEffect", 0f);
        Inst.SoundManager.SetAudioVolume(ESoundType.Effect, value);
    }

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
    
    public void SaveDisplayData()
    {
        string path = Path.Combine(Application.persistentDataPath, "DisplaySaveData.json");
        SaveResolution resolution = new SaveResolution(ScreenWidth, ScreenHeight, IsFullscreen);
        string resolutionJson = JsonUtility.ToJson(resolution);
        File.WriteAllText(path, resolutionJson);
    }
    
    public void LoadDisplayData()
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
    
    
}

[Serializable]
public class PlayerInfo
{
    public PlayerInfo(string inSceneName, Vector2 inPos, float inCurrentHp, 
        Dictionary<Define.ESkillId, SO_PassiveSkill> inPassiveSkills,
        int[] inActiveSkillLevels)
    {
        sceneName = inSceneName;
        pos = inPos;
        currentHp = inCurrentHp;
        activeSkillLevels = inActiveSkillLevels;
        Convert(inPassiveSkills);
    }
    [SerializeField] public string sceneName;
    [SerializeField] public Vector2 pos;
    [SerializeField] public float currentHp;
    [SerializeField] public PassiveSkillDictionary passiveSkills;
    [SerializeField] public int[] activeSkillLevels;

    private void Convert(Dictionary<Define.ESkillId, SO_PassiveSkill> inPassiveSkills)//, Dictionary<Define.ESkillId, SO_PassiveSkill> inEquippedPassiveSkills)
    {
        passiveSkills = new PassiveSkillDictionary();
        foreach (KeyValuePair<Define.ESkillId, SO_PassiveSkill> pair in inPassiveSkills)
        {
            PassiveSaveInfo passiveSaveInfo = new PassiveSaveInfo()
            {
                skillId = pair.Key, level = pair.Value.skillLevel, bCanLevelUp = pair.Value.bCanLevelUp, equipIndex = pair.Value.equipIndex
            };
            passiveSkills.Add(pair.Key, passiveSaveInfo);
        }
        passiveSkills.Serialize();
    }

    public void Deserialize()
    {
        passiveSkills.Deserialize();
    }
}


[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void Serialize()
    {
        keys.Clear();
        values.Clear();
        
        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void Deserialize()
    {
        Clear();
        
        if (keys.Count != values.Count)
            Debug.Log("error");

        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}