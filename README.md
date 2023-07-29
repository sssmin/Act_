# Act 포트폴리오



> 아래 이미지 클릭하면 유튜브로 이동합니다. 약 7분 길이의 영상입니다. <br>

[![Image](https://user-images.githubusercontent.com/27758519/230600186-e4bb837b-e38f-4dfa-9073-b8daaf09665c.jpg)](https://youtu.be/VU1xKQi2RtI)



<br>
엔진 : UnityEngine 2021.3.17f1 <br>
제작기간 : 약 1달 반 <br>
개발 규모 : 1인 개발 <br>

# 게임 설명
+ 던전 스테이지를 클리어하고, 플레이어를 성장시켜 나가는 어드벤처, RPG 요소를 결합한 2D 게임입니다.


# 주요 클래스 구현 설명

### GI 클래스
+ 싱글톤. 전역으로 접근할 수 있도록 구현, DontDestroyOnLoad 함수를 이용하여 씬을 이동하더라도 파괴되지 않고 유지되도록 함.
+ 매니저들, 현재 게임 상태, 플레이어 등을 멤버로 가짐.
+ 게임 데이터를 초기화하는 기능과 세이브, 로드 기능, 매니저들에 접근하는 용도로 주로 사용되는 클래스.

```c#
private void InitializeOnce()
{
  ...
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
      Inst.SoundManager.PlayBackgroundSound("Title");
      //4. UI 초기화
      InitializeUI();
      //5. 타이틀 메뉴 생성
      Inst.ResourceManager.Instantiate("UI_TitleMainMenu");
  });
}
```
+ 게임 실행 후 타이틀 씬이 로드됐을 때 단 한번만 호출되는 InitializeOnce 함수.
```c#
public void SaveSoundData()
{
    Inst.SoundManager.GetAudioVolume(ESoundType.Master, out float value);
    PlayerPrefs.SetFloat("AudioMaster", value);
...

private void LoadSoundData()
{
  float value = PlayerPrefs.GetFloat("AudioMaster", 0f);
  Inst.SoundManager.SetAudioVolume(ESoundType.Master, value);
...
```
+ 세이브는 사운드의 경우 PlayerPrefs 클래스를 이용하여 간단히 key와 value를 저장하도록 함.
```c#
private void SaveInventoryData()
{
    Serializable.S_InventoryInfo inventoryInfo = Player.InventoryManager.GetSerializeInventoryInfo();
    
    string path = Path.Combine(Application.persistentDataPath, "InventorySaveData.json");
    string jsonData = JsonUtility.ToJson(inventoryInfo);
    File.WriteAllText(path, jsonData);
}

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

```
+ 그외 데이터들은 json으로 파싱 후 저장하도록 함.
+ Dictionary 경우 기본적으로 직렬화가 안되기 때문에 따로 클래스를 정의하여 사용
```c#
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
...
[Serializable]
public class EtcDictionary : SerializableDictionary<string, Serializable.StackableItemSaveInfo_Lite> { }
```
+ 기본 Dictionary를 사용하다가, 저장할 때 커스텀 Dictionary를 이용하여 직렬화 후 저장.
+ 반대로 로드할 때는 커스텀 Dictionary를 역직렬화하여 초기화.
  
### ResourceManager 클래스
+ 리소스를 관리하는 클래스
+ 리소스들은 Addressables 시스템을 이용. 비동기 로드하여, 인게임에서 빠르게 접근해야 하는 리소스는 리소스 매니저에 미리 저장.
```c#
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
                completedCount++;
            }
            else
            {
                Debug.Log("download failed.");
            }

            if (completedCount == totalCount)
            {
                SaveData<ScriptableObject>(Define.ELabel.Skill);
      ....
```
+ DownloadDependenciesAsync 함수, LoadResourceLocationsAsync 함수, LoadAssetAsync 함수를 이용하여 리소스를 사전에 비동기로 로드하고, Dictionary에 저장.
+ 자주 사용될 프리팹은 오브젝트 풀링을 이용, PoolManager에서 관리하고 나머지는 리소스 매니저에 저장
+ 아이템 정보, 몬스터 정보 등 ScriptableObject를 이용하여 관리 (경우에 따라 원본을 사용하기도 하고, 복사본을 사용하기도 함)
+ 비동기 로드 후에 호출되어야 할 부분은 콜백 함수를 이용하여 처리.

### InventoryManager 클래스
+ 아이템을 관리하는 클래스
+ 쌓이지 않는 아이템 종류인 장비는 List로 관리, 그 외 아이템들은 Dictionary로 관리.
+ 최대로 쌓이는 갯수로 아이템 칸수를 분리하고 편하게 사용할 수 있도록 아이템 ScriptableObject를 포함하는 클래스를 따로 정의하여 Dictionary의 value값으로 사용.
``` c#
[Serializable]
public class StackableItem
{
   [SerializeField] public SO_Item item;
   [SerializeField] public List<int> amounts = new List<int>();
      
   public void AddAmount(int inAmount)
   {
      if (amounts.Count <= 0)
      {
         amounts.Add(0);
         GI.Inst.ListenerManager.IncreaseCurrentInventoryNum();
      }
      if ((amounts[amounts.Count - 1] + inAmount) > item.maxStackSize)
      {
         int canStackAmount = item.maxStackSize - amounts[amounts.Count - 1]; 
         int remainStackAmount = inAmount - canStackAmount; 
         amounts[amounts.Count - 1] = item.maxStackSize;

         amounts.Add(0);
         GI.Inst.ListenerManager.IncreaseCurrentInventoryNum();
            
         AddAmount(remainStackAmount);
      }
      else
      {
         amounts[amounts.Count - 1] += inAmount;
      }
   }
......

private Dictionary<SO_Item.EConsumableType, StackableItem> ConsumableInventory { get; set; } = new Dictionary<SO_Item.EConsumableType, StackableItem>();
```

### SkillManager 클래스
+
