using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;



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
        GI.Inst.ListenerManager.isAnyEquippedPassiveSkill -= IsAnyEquippedPassiveSkill;
        GI.Inst.ListenerManager.isAnyEquippedPassiveSkill += IsAnyEquippedPassiveSkill;
    }

    private void OnDestroy()
    {
        GI.Inst.ListenerManager.onExecuteActiveSkill -= ExecuteActiveSkill;
        GI.Inst.ListenerManager.getSkill -= GetSkill;
        GI.Inst.ListenerManager.onExecPlayerClone -= ExecPlayerClone;
        GI.Inst.ListenerManager.onExecEarthquake -= ExecEarthquake;
        GI.Inst.ListenerManager.setActiveSkillCuzEquip -= SetActiveSkillCuzEquip;
        GI.Inst.ListenerManager.isSkillReady -= IsSkillReady;
        GI.Inst.ListenerManager.getCurrentActiveSkills -= GetCurrentActiveSkills;
        GI.Inst.ListenerManager.getAllPassiveSkills -= GetAllPassiveSkills;
        GI.Inst.ListenerManager.requestActiveSkillLevelUp -= RequestActiveSkillLevelUp;
        GI.Inst.ListenerManager.requestPassiveSkillLevelUp -= RequestPassiveSkillLevelUp;
        GI.Inst.ListenerManager.getActiveSkillLevel -= GetActiveSkillLevel;
        GI.Inst.ListenerManager.equipPassiveSkill -= EquipPassiveSkill;
        GI.Inst.ListenerManager.unequipPassiveSkill -= UnequipPassiveSkill;
        GI.Inst.ListenerManager.execTakeDamageEffect -= ExecTakeDamageEffect;
        GI.Inst.ListenerManager.isAnyEquippedPassiveSkill -= IsAnyEquippedPassiveSkill;
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
                GI.Inst.CooltimeManager.FourthSkillTimer = Time.time + cooltime;
                GI.Inst.UIManager.SetSkillCooltimeUI(EActiveSkillOrder.Fourth, cooltime);
                break;
            case EActiveSkillOrder.Fifth:
                GI.Inst.CooltimeManager.FifthSkillTimer = Time.time + cooltime;
                GI.Inst.UIManager.SetSkillCooltimeUI(EActiveSkillOrder.Fifth, cooltime);
                break;
        }
    }

    private void SetActiveSkillCuzEquip(SO_Item.EWeaponType type)
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
                Debug.Log("이미 장착됨");
            else
                PassiveSkills[skillId].equipIndex = equipIndex;
        }
        else
        {
            Debug.Log("존재하지 않는 패시브.");
        }
    }
    
    private void UnequipPassiveSkill(Define.ESkillId skillId)
    {
        if (PassiveSkills.ContainsKey(skillId))
            PassiveSkills[skillId].equipIndex = -1;
        else
            Debug.Log("존재하지 않는 패시브.");
    }
    
    private bool IsSkillReady(EActiveSkillOrder skillOrder)
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
                return (GI.Inst.CooltimeManager.FourthSkillTimer <= Time.time);
            case EActiveSkillOrder.Fifth:
                return (GI.Inst.CooltimeManager.FifthSkillTimer <= Time.time);
        }
        return false;
    }

    private void UpdateActiveSkill()
    {
        int i = 0;
        foreach (KeyValuePair<ActiveSkillIdentify,SO_ActiveSkill> pair in ActiveSkills)
        { 
            pair.Value.EquipInit(ActiveSkillLevels[i++], GI.Inst.Player.StatManager);
        }
    }
    
    private void RequestActiveSkillLevelUp(ActiveSkill_Lite skill, int index)
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
    
    private void RequestPassiveSkillLevelUp(PassiveSkill_Lite skill)
    {
        if (PassiveSkills.ContainsKey(skill.skillId))
        {
            GI.Inst.ListenerManager.UsePassiveSkillMat(ref skill);
            PassiveSkills[skill.skillId].skillLevel++;

            PassiveSkills[skill.skillId].Init();
            GI.Inst.UIManager.RefreshPassiveSkillUI(GetAllPassiveSkills());
        }
    }

    public void CauseDamageSuccessfully(Define.EDamageType causeDamageType, ETakeDamageResult takeDamageResult,
        StatManager victimStatManager)
    {
        ExecCauseDamageEffect(causeDamageType, takeDamageResult, victimStatManager);
    }
    
    //플레이어만 염두에 두고 구현하면 됨. 몬스터는 SkillManager가 없음.
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
                    Effect effect = passiveSkill.effect;
                    
                    effect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.CauseDamage, 
                        victimStatManager, Player.StatManager, passiveSkill.icon);
                }
            }
        }

        ExecWeaponEffect(causeDamageType, victimStatManager);
        ExecWeaponElementEffect(causeDamageType, victimStatManager);
    }

    //무기가 가진 부가효과 실행 (ex. 인피니티 무기 피흡)
    private void ExecWeaponEffect(Define.EDamageType causeDamageType, StatManager victimStatManager)
    {
        SO_BaseWeapon equippedWeapon = GI.Inst.ListenerManager.GetEquippedWeapon();
        if (equippedWeapon == null) return;
        foreach (Effect effect in equippedWeapon.effects)
        {
            effect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.CauseDamage, victimStatManager, Player.StatManager, null);
        }
    }

    //무기가 가진 속성효과 실행 (ex. 불 속성 방감)=>상대방의 현재 스탯에 디버프 효과를 주는 것이기 때문에 다른 효과들과 다르게 피해 입힐때마다 Effect 생성.
    private void ExecWeaponElementEffect(Define.EDamageType causeDamageType, StatManager victimStatManager)
    {
        SO_BaseWeapon equippedWeapon = GI.Inst.ListenerManager.GetEquippedWeapon();
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

                    float value = 5f;
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

                    float value = victimStatManager.characterStats.attack.Value * 5f * 0.01f;
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

    private void ExecTakeDamageEffect(Define.EDamageType causeDamageType, StatManager instigatorStatManager)
    {
        foreach (var pair in PassiveSkills)
        {
            SO_PassiveSkill passiveSkill = pair.Value;
            if (passiveSkill.equipIndex != -1)
            {
                if (GI.Inst.CooltimeManager.IsReadyPassive(pair.Key))
                {
                    passiveSkill.ExecSkill(Player.StatManager, Player.PlayerController);
                    Effect effect = pair.Value.effect;
                    
                    effect.CheckConditionAndExecute(causeDamageType, Define.EActivationCondition.TakeDamage,
                        instigatorStatManager, Player.StatManager, pair.Value.icon);
                }
            }
        }
    }
    
    public List<SO_PassiveSkill> GetEquippedPassiveSkills()
    {
        List<SO_PassiveSkill> equippedPassiveSkills = new List<SO_PassiveSkill>();
        foreach (var pair in PassiveSkills)
        {
            if (pair.Value.equipIndex != -1)
            {
                equippedPassiveSkills.Add(pair.Value);
            }
        }

        return equippedPassiveSkills;
    }

    private bool IsAnyEquippedPassiveSkill()
    {
        foreach (var pair in PassiveSkills)
        {
            if (pair.Value.equipIndex != -1)
                return true;
        }
        return false;
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
    
    public Define.ESkillId[] GetActiveSkillType(SO_Item.EWeaponType type)
    {
        Define.ESkillId[] skillIds = new Define.ESkillId[5];
        
        switch (type)
        {
            case SO_Item.EWeaponType.Dagger:
                skillIds[0] = Define.ESkillId.ThrowDagger;
                skillIds[1] = Define.ESkillId.PlayerClone;
                skillIds[2] = Define.ESkillId.DaggerBall;
                skillIds[3] = Define.ESkillId.DaggerUlt;
                skillIds[4] = Define.ESkillId.Dash;
                break;
            case SO_Item.EWeaponType.Bow:
                skillIds[0] = Define.ESkillId.ArrowRain;
                skillIds[1] = Define.ESkillId.PiercingArrow;
                skillIds[2] = Define.ESkillId.ArrowBuff;
                skillIds[3] = Define.ESkillId.DistortionArrow;
                skillIds[4] = Define.ESkillId.Dash;
                break;
            case SO_Item.EWeaponType.Axe:
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
    
    
    #endregion
    #region Serialize

    public void SetDeserializeSkillInfo(Serializable.S_PlayerInfo playerInfo)
    {
        playerInfo.Deserialize();

        PassiveSkillDictionary skillDictionary = playerInfo.passiveSkills;
        foreach (KeyValuePair<Define.ESkillId, Serializable.PassiveSaveInfo_Lite> pair in skillDictionary)
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

    #endregion
}
