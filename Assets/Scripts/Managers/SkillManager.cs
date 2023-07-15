using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class PassiveSkillDictionary : SerializableDictionary<Define.ESkillId, PassiveSaveInfo>
{
}


[Serializable]
public class PassiveSaveInfo
{
    [SerializeField] public Define.ESkillId skillId;
    [SerializeField] public int level;
    [SerializeField] public bool bCanLevelUp;
    [SerializeField] public int equipIndex;

    public void SetInfo(SO_PassiveSkill passiveSkill)
    {
        skillId = passiveSkill.skillId;
        level = passiveSkill.skillLevel;
        bCanLevelUp = passiveSkill.bCanLevelUp;
        equipIndex = passiveSkill.equipIndex;
    }
}


public struct ActiveSkillIdentify : IEquatable<ActiveSkillIdentify>
{
    public EActiveSkillOrder skillOrder;
    public Define.ESkillId skillId;
    
    public bool Equals(ActiveSkillIdentify other)
    {
        return skillOrder == other.skillOrder || skillId == other.skillId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)skillOrder, (int)skillId);
    }
}


//player only
public class SkillManager : MonoBehaviour
{
    private Dictionary<ActiveSkillIdentify, SO_ActiveSkill> ActiveSkills { get; set; } = new Dictionary<ActiveSkillIdentify, SO_ActiveSkill>();
    public int[] ActiveSkillLevels { get; private set; } = new int[5] { 1, 1, 1, 1, 1 };

    public Dictionary<Define.ESkillId, SO_PassiveSkill> PassiveSkills { get; private set; } =
        new Dictionary<Define.ESkillId, SO_PassiveSkill>();

    private Player Player { get; set; }
    private float UltSkillChargeAmount { get; set; }
    

    private void Awake()
    {
        Player = GetComponent<Player>();
        BindAction();
    }

    private void BindAction()
    {
        GI.Inst.ListenerManager.onExecuteActiveSkill -= ExecuteActiveSkill;
        GI.Inst.ListenerManager.onExecuteActiveSkill += ExecuteActiveSkill;
        GI.Inst.ListenerManager.getSkill -= GetSkill;
        GI.Inst.ListenerManager.getSkill += GetSkill;
        GI.Inst.ListenerManager.onExecPlayerClone -= ExecPlayerClone;
        GI.Inst.ListenerManager.onExecPlayerClone += ExecPlayerClone;
        GI.Inst.ListenerManager.onExecEarthquake -= ExecEarthquake;
        GI.Inst.ListenerManager.onExecEarthquake += ExecEarthquake;
        GI.Inst.ListenerManager.setActiveSkillCuzEquip -= SetActiveSkillCuzEquip;
        GI.Inst.ListenerManager.setActiveSkillCuzEquip += SetActiveSkillCuzEquip;
        GI.Inst.ListenerManager.isSkillReady -= IsSkillReady;
        GI.Inst.ListenerManager.isSkillReady += IsSkillReady;
        GI.Inst.ListenerManager.getCurrentActiveSkills -= GetCurrentActiveSkills;
        GI.Inst.ListenerManager.getCurrentActiveSkills += GetCurrentActiveSkills;
        GI.Inst.ListenerManager.getAllPassiveSkills -= GetAllPassiveSkills;
        GI.Inst.ListenerManager.getAllPassiveSkills += GetAllPassiveSkills;
        GI.Inst.ListenerManager.requestActiveSkillLevelUp -= RequestActiveSkillLevelUp;
        GI.Inst.ListenerManager.requestActiveSkillLevelUp += RequestActiveSkillLevelUp;
        GI.Inst.ListenerManager.requestPassiveSkillLevelUp -= RequestPassiveSkillLevelUp;
        GI.Inst.ListenerManager.requestPassiveSkillLevelUp += RequestPassiveSkillLevelUp;
        GI.Inst.ListenerManager.getActiveSkillLevel -= GetActiveSkillLevel;
        GI.Inst.ListenerManager.getActiveSkillLevel += GetActiveSkillLevel;
        GI.Inst.ListenerManager.equipPassiveSkill -= EquipPassiveSkill;
        GI.Inst.ListenerManager.equipPassiveSkill += EquipPassiveSkill;
        GI.Inst.ListenerManager.unequipPassiveSkill -= UnequipPassiveSkill;
        GI.Inst.ListenerManager.unequipPassiveSkill += UnequipPassiveSkill;
        GI.Inst.ListenerManager.execTakeDamageEffect -= ExecTakeDamageEffect;
        GI.Inst.ListenerManager.execTakeDamageEffect += ExecTakeDamageEffect;
        GI.Inst.ListenerManager.getUltSkillChargeAmount -= GetUltSkillChargeAmount;
        GI.Inst.ListenerManager.getUltSkillChargeAmount += GetUltSkillChargeAmount;
        GI.Inst.ListenerManager.initUltSkillChargeAmount -= InitUltSkillChargeAmount;
        GI.Inst.ListenerManager.initUltSkillChargeAmount += InitUltSkillChargeAmount;
    }
    
    public void SetStartPassiveSkills()
    {
        List<SO_PassiveSkill> passiveSkills = new List<SO_PassiveSkill>();
        for (int i = (int)Define.ESkillId.HealthSteal; i < (int)Define.ESkillId.Max; i++)
        {
            SO_PassiveSkill skill = GI.Inst.ResourceManager.GetPassiveSkillData((Define.ESkillId)i);
            skill.skillLevel = 1;
            skill.Init();
            skill.ExecSkill(Player.StatManager, Player.PlayerController);
            PassiveSkills.Add((Define.ESkillId)i, skill);
            passiveSkills.Add(skill);
        }
        GI.Inst.UIManager.RefreshPassiveSkillUI(passiveSkills);
    }

    void ExecuteActiveSkill(int instanceId, Define.ESkillId skillId)
    {
        if (Player.InstId != instanceId) return;
        
        foreach (var pair in ActiveSkills)
        {
            if (pair.Key.skillId == skillId)
            {
                ActiveSkillSetCooltime(pair.Key.skillOrder, pair.Value.skillCooltime);
                pair.Value.ExecSkill(Player.StatManager, Player.PlayerController);
                break;
            }
        }
    }

    void ActiveSkillSetCooltime(EActiveSkillOrder skillOrder, float cooltime)
    {
        float skillCooltimeDecRate = Player.StatManager.characterStats.skillCooltimeDecRate.Value;
        cooltime -= cooltime * skillCooltimeDecRate * 0.01f;
        switch (skillOrder)
        {
            case EActiveSkillOrder.First:
                GI.Inst.CooltimeManager.FirstSkillTimer = Time.time + cooltime;
                GI.Inst.UIManager.SetSkillCooltimeUI(EActiveSkillOrder.First, cooltime);
                break;
            case EActiveSkillOrder.Second:
                GI.Inst.CooltimeManager.SecondSkillTimer = Time.time + cooltime;
                GI.Inst.UIManager.SetSkillCooltimeUI(EActiveSkillOrder.Second, cooltime);
                break;
            case EActiveSkillOrder.Third:
                GI.Inst.CooltimeManager.ThirdSkillTimer = Time.time + cooltime;
                GI.Inst.UIManager.SetSkillCooltimeUI(EActiveSkillOrder.Third, cooltime);
                break;
            case EActiveSkillOrder.Fourth:
                UltSkillChargeAmount = 0f;
                break;
            case EActiveSkillOrder.Fifth:
                GI.Inst.CooltimeManager.FifthSkillTimer = Time.time + cooltime;
                GI.Inst.UIManager.SetSkillCooltimeUI(EActiveSkillOrder.Fifth, cooltime);
                break;
        }
    }

    public void SetActiveSkillCuzEquip(Item.EWeaponType type)
    {
        Define.ESkillId[] skillIds = GetActiveSkillType(type);
        
        SetActiveSkill(skillIds);
    }

    private void SetActiveSkill(Define.ESkillId[] skillIds)
    {
        ActiveSkills.Clear();
        List<SO_ActiveSkill> skills = new List<SO_ActiveSkill>();
        List<Sprite> skillIcons = new List<Sprite>();
        for (int i = 0; i < skillIds.Length; i++)
        {
            ActiveSkillIdentify identify = new ActiveSkillIdentify();
            identify.skillOrder = (EActiveSkillOrder)i;
            identify.skillId = skillIds[i];
            SO_ActiveSkill skill = GI.Inst.ResourceManager.GetActiveSkillData(skillIds[i]);
            skill.EquipInit(ActiveSkillLevels[i], GI.Inst.Player.StatManager);
            skills.Add(skill);
            skillIcons.Add(skill.icon);
            ActiveSkills.Add(identify, skill);
        }
        GI.Inst.ListenerManager.CheckSkillMatCanLevelUpSkills(skills);
        GI.Inst.UIManager.RefreshSkillHotkeyMainUI(skillIcons);
    }
    

    private void EquipPassiveSkill(Define.ESkillId skillId, int equipIndex)
    {
        if (PassiveSkills.ContainsKey(skillId))
        {
            if (PassiveSkills[skillId].equipIndex != -1)
            {
                Debug.Log("이미 장착됨");
            }
            else
            {
                PassiveSkills[skillId].equipIndex = equipIndex;
            }
        }
        else
        {
            Debug.Log("존재하지 않는 패시브.");
        }
    }
    
    private void UnequipPassiveSkill(Define.ESkillId skillId)
    {
        if (PassiveSkills.ContainsKey(skillId))
        {
            PassiveSkills[skillId].equipIndex = -1;
        }
        else
        {
            Debug.Log("존재하지 않는 패시브.");
        }
    }

    public void ChargeFourthSkill(Define.EDamageType causeDamageType)
    {
        float randValue = 0f;
        if (causeDamageType == Define.EDamageType.Normal)
        {
            randValue = Random.Range(1f, 3f);
        }
        else if (causeDamageType == Define.EDamageType.Skill)
        {
            randValue = Random.Range(0.1f, 0.3f);
        }
        
        UltSkillChargeAmount = Mathf.Clamp(UltSkillChargeAmount + randValue, 0f, 100f);
        
        GI.Inst.UIManager.UpdateFillAmount(UltSkillChargeAmount);
        
        if (UltSkillChargeAmount >= 100f)
        {
            //충전완료
            //완료된 애니메이션? 알림?
        }
    }

    public bool IsSkillReady(EActiveSkillOrder skillOrder)
    {
        switch (skillOrder)
        {
            case EActiveSkillOrder.First:
                return (GI.Inst.CooltimeManager.FirstSkillTimer <= Time.time);
            case EActiveSkillOrder.Second:
                return (GI.Inst.CooltimeManager.SecondSkillTimer <= Time.time);
            case EActiveSkillOrder.Third:
                return (GI.Inst.CooltimeManager.ThirdSkillTimer <= Time.time);
            case EActiveSkillOrder.Fourth:
                return (UltSkillChargeAmount >= 100f);
            case EActiveSkillOrder.Fifth:
                return (GI.Inst.CooltimeManager.FifthSkillTimer <= Time.time);
        }
        return false;
    }

    public void UpdateActiveSkill()
    {
        int i = 0;
        foreach (KeyValuePair<ActiveSkillIdentify,SO_ActiveSkill> pair in ActiveSkills)
        { 
            pair.Value.EquipInit(ActiveSkillLevels[i++], GI.Inst.Player.StatManager);
        }
    }
    
    public void RequestActiveSkillLevelUp(ActiveSkill_ShortVer skill, int index)
    {
        if (skill.activeSkillOrder != EActiveSkillOrder.Max) 
        {
            ActiveSkillIdentify skillIdentify = new ActiveSkillIdentify();
            skillIdentify.skillId = skill.skillId;
            skillIdentify.skillOrder = skill.activeSkillOrder;
            if (ActiveSkills.ContainsKey(skillIdentify))
            {
                GI.Inst.ListenerManager.UseActiveSkillMat(skill.itemIdForLevelUp, skill.activeSkillOrder);
                ActiveSkillLevels[index]++;
                UpdateActiveSkill();
                GI.Inst.UIManager.RefreshActiveSkillUI(GetCurrentActiveSkills());
            }
        }
    }
    
    public void RequestPassiveSkillLevelUp(PassiveSkill_ShortVer skill)
    {
        if (PassiveSkills.ContainsKey(skill.skillId))
        {
            GI.Inst.ListenerManager.UsePassiveSkillMat(ref skill);
            PassiveSkills[skill.skillId].skillLevel++;

            PassiveSkills[skill.skillId].Init();
            GI.Inst.UIManager.RefreshPassiveSkillUI(GetAllPassiveSkills());
        }
    }
    
    //장착되었으면 을 확인 하고 되어있으면 해제 후 다시 착용
    public void ReEquipPassive(Define.ESkillId skillId)
    {
        if (PassiveSkills.ContainsKey(skillId))
        {
            if (PassiveSkills[skillId].equipIndex != -1)
            {
                UnequipPassiveSkill(skillId);
                EquipPassiveSkill(skillId, PassiveSkills[skillId].equipIndex);
            }
        }
    }

    public void CauseDamageSuccessfully(Define.EDamageType causeDamageType, ETakeDamageResult takeDamageResult,
        StatManager victimStatManager)
    {
        ExecCauseDamageEffect(causeDamageType, takeDamageResult, victimStatManager);
        ChargeFourthSkill(causeDamageType);
    }
    
    //플레이어만 염두에 두고 구현하면 됨. 몬스터는 SkillManager가 없음.
    public void ExecCauseDamageEffect(Define.EDamageType causeDamageType, ETakeDamageResult takeDamageResult, StatManager victimStatManager)
    {
        foreach (var pair in PassiveSkills)
        {
            SO_PassiveSkill passvieSkill = pair.Value;
            if (passvieSkill.equipIndex != -1)
            {
                if (GI.Inst.CooltimeManager.IsReadyPassive(pair.Key))
                {
                    passvieSkill.ExecSkill(Player.StatManager, Player.PlayerController);
                    Effect effect = passvieSkill.effect;
                    
                    effect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.CauseDamage, 
                        victimStatManager, Player.StatManager, passvieSkill.icon);
                }
            }
        }

        ExecWeaponElementEffect(causeDamageType, victimStatManager);
    }

    private void ExecWeaponElementEffect(Define.EDamageType causeDamageType, StatManager victimStatManager)
    {
        BaseWeapon equippedWeapon = GI.Inst.ListenerManager.GetEquippedWeapon();
        if (equippedWeapon == null) return;
        if (equippedWeapon.Element == EWeaponElement.None) return;
        
        EDurationEffectId durationEffectId = EDurationEffectId.None;
        DurationEffect weaponEffect = null;
        EffectInfo effectInfo = new EffectInfo();
        if (equippedWeapon)
        {
            switch (equippedWeapon.Element)
            {
                case EWeaponElement.Fire:
                {
                    weaponEffect = new DurationEffect_Debuff_ArmorDecrease();
                    durationEffectId = EDurationEffectId.Burn;

                    float value = victimStatManager.characterStats.defence.Value * 5f * 0.01f;
                    effectInfo.onExecuteIncreaseStat = () =>
                    {
                        victimStatManager.characterStats.defenceIncValue.AddModifier(-value);
                    };

                    effectInfo.onExecuteDecreaseStat = () =>
                    {
                        victimStatManager.characterStats.defenceIncValue.SubModifier(-value);
                    };
                }
                    break;
                case EWeaponElement.Water:
                {
                    weaponEffect = new DurationEffect_Debuff_CriticalResistDecrease();
                    durationEffectId = EDurationEffectId.Frozen;

                    float value = victimStatManager.characterStats.criticalResistPer.Value * 5f * 0.01f;
                    effectInfo.onExecuteIncreaseStat = () =>
                    {
                        victimStatManager.characterStats.criticalResistPer.AddModifier(-value);
                    };

                    effectInfo.onExecuteDecreaseStat = () =>
                    {
                        victimStatManager.characterStats.criticalResistPer.SubModifier(-value);
                    };
                }
                    break;
                case EWeaponElement.Leaf:
                {
                    weaponEffect = new DurationEffect_Debuff_AttackDecrease();
                    durationEffectId = EDurationEffectId.Poison;

                    float value = victimStatManager.characterStats.attackIncValue.Value * 5f * 0.01f;
                    effectInfo.onExecuteIncreaseStat = () =>
                    {
                        victimStatManager.characterStats.attackIncValue.AddModifier(-value);
                    };

                    effectInfo.onExecuteDecreaseStat = () =>
                    {
                        victimStatManager.characterStats.attackIncValue.SubModifier(-value);
                    };
                }
                    break;
            }

            weaponEffect.Init(Define.EActivationCondition.CauseDamage, 10f, Define.EDamageType.Both, effectInfo, 5f,
                durationEffectId, true);
            Sprite statusIcon = GI.Inst.ResourceManager.GetStatusSprite(Enum.GetName(typeof(EDurationEffectId), durationEffectId));
            weaponEffect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.CauseDamage,
                Player.StatManager, victimStatManager, statusIcon);
        }
    }

    public void ExecTakeDamageEffect(Define.EDamageType causeDamageType, StatManager instigatorStatManager)
    {
        foreach (var pair in PassiveSkills)
        {
            if (pair.Value.equipIndex != -1)
            {
                if (GI.Inst.CooltimeManager.IsReadyPassive(pair.Key))
                {
                    Effect effect = pair.Value.effect;
                    effect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.TakeDamage,
                        instigatorStatManager, Player.StatManager, pair.Value.icon);
                }
            }
        }
    }

    public void SetDeserializeSkillInfo(PlayerInfo playerInfo)
    {
        playerInfo.Deserialize();

        PassiveSkillDictionary skillDictionary = playerInfo.passiveSkills;
        foreach (KeyValuePair<Define.ESkillId, PassiveSaveInfo> pair in skillDictionary)
        {
            SO_PassiveSkill passiveSkill = GI.Inst.ResourceManager.GetPassiveSkillData(pair.Key);
            passiveSkill.skillLevel = pair.Value.level;
            passiveSkill.bCanLevelUp = pair.Value.bCanLevelUp;
            passiveSkill.equipIndex = pair.Value.equipIndex;
            passiveSkill.Init();
            passiveSkill.ExecSkill(Player.StatManager, Player.PlayerController);
            PassiveSkills.Add(pair.Key, passiveSkill);
            if (passiveSkill.equipIndex != -1)
            {
                GI.Inst.UIManager.SetEquipPassive(passiveSkill);
            }
        }
        
        ActiveSkillLevels = playerInfo.activeSkillLevels;
        GI.Inst.UIManager.RefreshActiveSkillUI(GetCurrentActiveSkills());
        GI.Inst.UIManager.RefreshPassiveSkillUI(GetAllPassiveSkills());
    }
    
    #region Clone
    public void ExecPlayerClone(int instanceId, StatManager castStatManager, DamageInfo damageInfo)
    {
        if (Player.InstId != instanceId) return;
        List<Transform> transforms = CombatManager.GetCloseEnemiesTransforms(castStatManager.transform, 5);
        StartCoroutine(CoSpawnClone(transforms, castStatManager, damageInfo));
    }

    IEnumerator CoSpawnClone(List<Transform> transforms, StatManager castStatManager, DamageInfo damageInfo)
    {
        foreach (Transform trans in transforms)
        {
            //todo 적 위치에 정확히 스폰하지말고 왼쪽 오른쪽 분산하여 스폰
            StatManager enemyStatManager = trans.GetComponent<StatManager>();

            GameObject go = GI.Inst.ResourceManager.Instantiate("PlayerClone", trans.position, quaternion.identity);
            SkillAbility_PlayerClone playerClone = go.GetComponent<SkillAbility_PlayerClone>();
            playerClone.Init(castStatManager, damageInfo, enemyStatManager);
            
            yield return new WaitForSeconds(0.2f);
        }
    }
    

    #endregion
    #region Earthquake
    public void ExecEarthquake(int instanceId, StatManager castStatManager, DamageInfo damageInfo)
    {
        if (Player.InstId != instanceId) return;
        StartCoroutine(CoSpawnEarthquake(castStatManager, damageInfo));
    }

    IEnumerator CoSpawnEarthquake(StatManager castStatManager, DamageInfo damageInfo)
    {
        Vector2 spawnPos = GetEarthquakeSpawnPos();
        if (spawnPos == new Vector2(10000, 10000)) 
            yield break;
        
        for (int i = 0; i < 3; i++)
        {
            GameObject go =
                GI.Inst.ResourceManager.Instantiate("Earthquake", spawnPos, quaternion.identity);
            SkillAbility_Earthquake earthquake = go.GetComponent<SkillAbility_Earthquake>();
            earthquake.Init(castStatManager, damageInfo);
            spawnPos.x = spawnPos.x + (earthquake.Sr.bounds.size.x * Player.PlayerController.CurrentDir.x);
           
           yield return new WaitForSeconds(0.5f);
        }
    }

    Vector3 GetEarthquakeSpawnPos()
    {
        RaycastHit2D hitResult =
            Physics2D.Raycast(Player.arrowSpawnPoint.position + new Vector3(Player.PlayerController.CurrentDir.x * 2f, 0f, 0f), Vector2.down, Player.CapsuleCollider.size.y, LayerMask.GetMask("Ground"));
        if (hitResult)
        {
            return hitResult.point;
        }
        return new Vector3(10000, 10000, 10000);
    }
    #endregion
    #region GetFunction
    
    public Define.ESkillId[] GetActiveSkillType(Item.EWeaponType type)
    {
        Define.ESkillId[] skillIds = new Define.ESkillId[5];
        
        switch (type)
        {
            case Item.EWeaponType.Dagger:
                skillIds[0] = Define.ESkillId.ThrowDagger;
                skillIds[1] = Define.ESkillId.PlayerClone;
                skillIds[2] = Define.ESkillId.DaggerBall;
                skillIds[3] = Define.ESkillId.DaggerUlt;
                skillIds[4] = Define.ESkillId.Dash;
                break;
            case Item.EWeaponType.Bow:
                skillIds[0] = Define.ESkillId.ArrowRain;
                skillIds[1] = Define.ESkillId.PiercingArrow;
                skillIds[2] = Define.ESkillId.ArrowBuff;
                skillIds[3] = Define.ESkillId.DistortionArrow;
                skillIds[4] = Define.ESkillId.Dash;
                break;
            case Item.EWeaponType.Axe:
                skillIds[0] = Define.ESkillId.FireStrike;
                skillIds[1] = Define.ESkillId.Earthquake;
                skillIds[2] = Define.ESkillId.ThrowAxe;
                skillIds[3] = Define.ESkillId.AxeUlt;
                skillIds[4] = Define.ESkillId.Dash;
                break;
        }

        return skillIds;
    }
    
    public List<SO_ActiveSkill> GetCurrentActiveSkills()
    {
        return ActiveSkills.Values.ToList();
    }

    public List<SO_PassiveSkill> GetAllPassiveSkills()
    {
        return PassiveSkills.Values.ToList();
    }
    
    public int GetActiveSkillLevel(EActiveSkillOrder skillOrder)
    {
        return ActiveSkillLevels[(int)skillOrder];
    }
    
    public SO_Skill GetSkill(EActiveSkillOrder skillOrder)
    {
        foreach (var pair in ActiveSkills)
        {
            if (pair.Key.skillOrder == skillOrder)
                return pair.Value;
        }
        return null;
    }

    public float GetUltSkillChargeAmount()
    {
        return UltSkillChargeAmount;
    }

    public void InitUltSkillChargeAmount()
    {
        UltSkillChargeAmount = 0;
        GI.Inst.UIManager.UpdateFillAmount(UltSkillChargeAmount);
    }
    
    #endregion
    
}
