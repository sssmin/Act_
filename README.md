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
+ 게임 전역에서 사용되는 전역 인스턴스를 관리하는 클래스
+ 싱글톤 패턴을 이용하여 게임의 전반적인 기능을 효율적으로 관리
+ DontDestroyOnLoad 함수를 이용하여 씬을 이동하더라도 파괴되지 않고 유지되도록 함.
+ 게임 데이터를 초기화하는 기능과 세이브, 로드 기능, 매니저들에 접근하는 용도로 주로 사용되는 클래스.
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
...
[Serializable]
public class EtcDictionary : SerializableDictionary<string, Serializable.StackableItemSaveInfo_Lite> { }
```
+ 기본 Dictionary를 사용하다가, 저장할 때 커스텀 Dictionary를 이용하여 직렬화 후 저장.
+ 반대로 로드할 때는 커스텀 Dictionary를 역직렬화하여 초기화.
  
### ResourceManager 클래스
+ 리소스 로딩 및 관리를 담당하는 클래스
+ Prefab, AudioClip, ScriptableObject 등의 리소스를 Addressables 시스템을 이용, 효율적으로 관리하여 게임 성능 향상
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
+ 자주 사용될 프리팹은 오브젝트 풀링을 이용, PoolManager에서 관리하고 나머지는 리소스 매니저에 저장
+ 아이템 정보, 몬스터 정보 등 ScriptableObject를 이용하여 관리 (경우에 따라 원본을 사용하기도 하고, 복사본을 사용하기도 함)
+ 비동기 로드 후에 호출되어야 할 부분은 콜백 함수를 이용하여 처리.

### InventoryManager 클래스
+ 플레이어 인벤토리의 아이템 관리 클래스
+ 아이템 추가, 삭제, 사용 등을 효율적으로 관리하여 플레이어 인벤토리 기능 제공
+ 쌓이지 않는 아이템 종류인 장비는 List로 관리, 그 외 아이템들은 Dictionary로 관리.
+ 인벤토리 칸 분리를 편하게 할 수 있도록 Item ScriptableObject를 포함하는 클래스를 따로 정의하여 Dictionary의 value값으로 사용.
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
+ 무기 종류에 따른 액티브 스킬 관리, 패시브 스킬 관리, 무기의 부가 효과 관리 클래스
+ 액티브 스킬의 경우 플레이어가 사용하고자 할 때 시전하면 되기 때문에 간단했으나, 패시브 스킬의 경우 조건부가 붙어야 해서 이전 포트폴리오때와 마찬가지로 많은 고민을 함..
  + 이전 포트폴리오와 비교하여 공격 및 피격 여부 확인에 if 문 대신, 스킬 시전 조건 및 효과를 ScriptableObject로 관리하여 유연한 스킬 시스템 제공
  + => 필요한 함수를 재정의(효과가 발동될 조건을 포함, 스킬 효과를 정의)하고 호출하도록 구현.
``` c#
private void ExecCauseDamageEffect(Define.EDamageType causeDamageType, ETakeDamageResult takeDamageResult, StatManager victimStatManager)
{
    foreach (var pair in PassiveSkills)
    {
        SO_PassiveSkill passiveSkill = pair.Value;
        if (passiveSkill.equipIndex != -1)
        {
            if (GI.Inst.CooltimeManager.IsReadyPassive(pair.Key))
            {
                passiveSkill.ExecSkill(Player.StatManager, Player.PlayerController);
                Effect effect = passvieSkill.effect;
                
                effect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.CauseDamage, 
                    victimStatManager, Player.StatManager, passiveSkill.icon);
    ...

//SO_Skill_DeadlyImpact.cs
public override void ExecSkill(StatManager casterStatManager, PlayerController playerController)
{
    EffectInfo effectInfo = new EffectInfo();
    effectInfo.applyPerBySkillLevel = criticalChance;
    
    effectInfo.onExecuteIncreaseStat = () => { casterStatManager.characterStats.criticalChancePer.AddModifier(effectInfo.applyPerBySkillLevel); };
    effectInfo.onExecuteDecreaseStat = () => { casterStatManager.characterStats.criticalChancePer.SubModifier(effectInfo.applyPerBySkillLevel); };
    durationEffect = new DurationEffect_DeadlyImpact();
    
    float duration = 5f;
    durationEffect.Init(Define.EActivationCondition.CauseDamage, -1f, Define.EDamageType.Normal, effectInfo, duration, EDurationEffectId.DeadlyImpact, true);
    durationEffect.skillCooltime = skillCooltime;
    effect = durationEffect;
}

//DurationEffect_DeadlyImpact.cs
public class DurationEffect_DeadlyImpact : DurationEffect
{
    public override void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition,
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        if (((damageType == Define.EDamageType.Both) || (damageType == inDamageType)) && activationCondition == inActivationCondition)
        {
            durationEndTime = duration + Time.time;
            casterStatManager.ExecDurationEffect(this, icon);
            GI.Inst.CooltimeManager.SetPassiveCooltime(Define.ESkillId.DeadlyImpact, skillCooltime);
            GI.Inst.UIManager.SetPassiveCooltimeSlot(Define.ESkillId.DeadlyImpact, icon);
```
+ 대미지를 받았을 때는 StatManager에서 똑같이 SkillManager의 ExecTakeDamageEffect 함수를 호출하는데 조건만 다르고 ExecCauseDamageEffect와 동일함.

### StatManager 클래스
+ 스탯을 관리하고, 플레이어의 경우 스탯 증가가 되는 스킬 효과의 지속 시간을 관리하는 클래스
  
``` c#
[Serializable]
public class Stat
{
    ...
    [SerializeField]
    private float statValue;
    public float Value
    {
        get
        {
            float finalValue = statValue;
            if (Modifiers.Count > 0)
            {
                foreach (var modi in Modifiers)
                {
                    finalValue += modi;
                }
            }
            return finalValue;
        }
        set => statValue = value;
        
    }

    private List<float> Modifiers = new List<float>();
    
    public void AddModifier(float value)
    {
        if (value != 0)
            Modifiers.Add(value);
    }
...

[Serializable]
public class Stats
{
    public Stat attack = new Stat(Define.EStatType.Attack);
    public Stat attackIncValue = new Stat(Define.EStatType.AttackIncValue);
    public Stat defence = new Stat(Define.EStatType.Defence);
...

public class StatManager : MonoBehaviour
{
    public Stats characterStats = new Stats();
...
```
+ Stats 클래스로 스탯을 관리하고, Stat 클래스에서 value와 버프나 아이템으로 증가된 수치를 Modifiers 리스트로 관리.

``` c#
//StatManager.cs
protected PriorityQueue<DurationEffect> DurationEffectEndTimePq { get; set; } = new PriorityQueue<DurationEffect>();

//DurationEffect.cs
public int CompareTo(DurationEffect other)
{
    if (ReferenceEquals(this, other)) return 0;
    if (ReferenceEquals(null, other)) return 1;
   
    return -durationEndTime.CompareTo(other.durationEndTime);
}
```

+ 지속 시간은 우선순위큐를 따로 정의해서 사용. Update 함수에서 가장 먼저 끝나는 지속 시간 효과를 Peek하여 체크하는 방식으로 구현.
``` c#
//PlayerStatManager.cs Update 함수
DurationEffect durationEffect = DurationEffectEndTimePq.Peek();
if (durationEffect.durationEndTime <= Time.time)//지속시간 끝
{
    durationEffect = DurationEffectEndTimePq.Pop();
...
```
+ 그외 액티브 스킬, 패시브 스킬, 아이템 쿨타임은 CooltimeManager를 이용하여 쿨타임 관리.
+ 쿨타임은 지속 시간과 다르게 아이콘에도 쿨타임 표시를 각각 해야줘야 해서 코루틴과 Dictionary로 관리.

### PlayerController
+ 플레이어의 모든 입력을 받고 처리하는 클래스
+ InputSystem을 이용하여 액션에 함수를 바인딩
+ 바인딩된 KeyCode를 UI에 표시, 설정을 통해 키 변경하는 기능
  + InputAction 클래스의 PerformInteractiveRebinding 함수를 이용하여 리바인딩
```c#
rebindingOperation = actionToRebind.PerformInteractiveRebinding()
        ...
        .OnPotentialMatch(operation =>
        {
            string inputKey = ConvertInputKey(operation.selectedControl.path); 
            string playerControlKey; 
    
            foreach (InputAction inputAction in PlayerControl)
            {
                foreach (InputBinding binding in inputAction.bindings) 
                {
                    string playerControlKey = ConvertPlayerControlKey(binding.effectivePath);
    
                    if (inputKey == playerControlKey)
                    {
                        ReplaceBinding(actionToRebind, binding, inputAction);
                        break;
                    }
                }
            }
        })
        .OnComplete(operation =>
        {
            RebindCompleted();
            actionToRebind.Enable();
        });
    
    rebindingOperation.Start();
}
```
[스크린샷]



### FSM
+ 플레이어와 몬스터의 상태를 FSM으로 관리하여 캐릭터의 상태 전환을 효율적으로 구현.
+ 각 상태 클래스에서 상태에 따른 로직을 구현하여 가독성이 높고 유지보수가 용이하도록 함.
``` c#
public class Player_WallSlideState : PlayerState
{
    ...
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.wallSlide, true);
    }
    
    public override void Update()
    {
        base.Update();

        if (!PlayerController.IsWallDetect())
        {
             TransitionState(Define.EPlayerState.Falling);
             return;
        }
        
        Rb.velocity = new Vector2(0, Rb.velocity.y * 0.7f);
        
        if (PlayerController.IsGroundDetect())
        { 
            TransitionState(Define.EPlayerState.Idle);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.wallSlide, false);
    }
}
```
### AI
+ AIController에서 몬스터 AI를 모두 제어 관리.
  + 플레이어가 몬스터의 BoxCollider에 Trigger되면 타겟팅, 추격 상태로 전환하고 추격 상태에서는 타겟과의 거리 계산 후 기본 공격이 되는 거리면 공격 (결국 State에서 해야할 일 정의 = FSM)
``` c#
private void OnTriggerEnter2D(Collider2D col)
{
    Player player = col.GetComponent<Player>();
    if (player)
    {
        Target = player;
        ControlledMonster.TransitionState(Define.EMonsterState.Chase);
````

### UI


### 포트폴리오를 준비하며 느낀 점
+ 2D 게임을 개발해보고 싶기도 했고, C#도 언젠가 공부해놓으면 좋을 거 같아서 이번에 처음으로 C#과 유니티를 공부했는데
  C++을 하다가 C#으로 넘어오니 포인터를 사용하지 않는 것이 오히려 초반에는 헷갈리는 부분이 많았다.
  그리고 C++에서는 되던 것이 C#에서는 안되거나 C++에서는 안되던 것이 C#에선 '어? 이게 되네?' 경우가 생각보다 많아서 굉장히 혼란스러웠다.
  인프런 강의와 구글링을 통해 공부했는데 금방 끝낼 수 있을 줄 알았으나 위의 이유로 생각보다 오래 걸렸다.

  어느 정도 익숙해진 후에는 언리얼로 개발할 때보다 더 편하고, 빠르게 개발할 수 있었던 것 같다.
  스크립트를 만들어서 기능들을 만들고, 게임 오브젝트에 붙여야 한다는 것이 처음에는 언리얼보다 어렵겠다라고 생각이 들었는데, 막상 해보고 익숙해지니 오히려 더 쉽고, 자유도가 있는 느낌이 들었다.

  또한 유니티도 언리얼 못지 않게 강력한 에디터라는 걸 알게 됐고, 오히려 언리얼보다도 편한 것이 많았던 것 같다. 특히 크래시...
  따로 공부하면서 느꼈던 것은 에디터 자체를 커스텀하는 것도 접근성이 좋다는 점.

  인디 게임 같이 작은 규모의 게임들을 왜 유니티로 많이 개발하는지 몸소 깨달았던 경험이었다.
