using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;



public struct SkillIdentify : IEquatable<SkillIdentify>
{
    public KeyCode keyCode;
    public Define.SkillId skillId;
    
    public bool Equals(SkillIdentify other)
    {
        return keyCode == other.keyCode || skillId == other.skillId;
    }

    public override int GetHashCode()
    {
        return keyCode.GetHashCode() ^ skillId.GetHashCode();
    }
}


public class SkillManager : MonoBehaviour
{
    private Dictionary<SkillIdentify, ActiveSkill> AvailableSkills { get; set; } = new Dictionary<SkillIdentify, ActiveSkill>();
    //todo 여기 맵에 넣을 때 Skill 복사본으로 넣어야함!

    private float qSkillTimer;
    private float wSkillTimer;
    private float eSkillTimer;
    private float rSkillTimer;
    
    
    private Player Player { get; set; }
    

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    void Start()
    {
        GI.Inst.ListenerManager.onExecuteSkill -= ExecuteSkill;
        GI.Inst.ListenerManager.onExecuteSkill += ExecuteSkill;
        GI.Inst.ListenerManager.getSkill -= GetSkill;
        GI.Inst.ListenerManager.getSkill += GetSkill;
        GI.Inst.ListenerManager.onExecPlayerClone -= ExecPlayerClone;
        GI.Inst.ListenerManager.onExecPlayerClone += ExecPlayerClone;
    }
    

    //무기를 변경했을 때 스킬 바꾸기.
    public void InitSkills()
    {
        //todo 인벤토리 추가되면 해야할 것
        //todo 무기에 저장되어있는 스킬 id 가져와서 
        //todo resource manager에서 스킬 id로 skills에서 skill SO 가져와야함 이때 복사본으로 해야한다.
        
        //test
        SkillIdentify identify = new SkillIdentify();
        identify.keyCode = KeyCode.Q;
        identify.skillId = Define.SkillId.ThrowDagger;
        ActiveSkill temp = GI.Inst.ResourceManager.GetSkillDataCopy(Define.SkillId.ThrowDagger) as ActiveSkill;
        temp.Init(1);
        AvailableSkills.Add(identify, temp);
        
        SkillIdentify identify2 = new SkillIdentify();
        identify2.keyCode = KeyCode.W;
        identify2.skillId = Define.SkillId.PlayerClone;
        ActiveSkill temp2 = GI.Inst.ResourceManager.GetSkillDataCopy(Define.SkillId.PlayerClone) as ActiveSkill;
        temp2.Init(1);
        AvailableSkills.Add(identify2, temp2);
        
        SkillIdentify identify3 = new SkillIdentify();
        identify3.keyCode = KeyCode.R;
        identify3.skillId = Define.SkillId.DaggerUlt;
        ActiveSkill temp3 = GI.Inst.ResourceManager.GetSkillDataCopy(Define.SkillId.DaggerUlt) as ActiveSkill;
        temp3.Init(1);
        AvailableSkills.Add(identify3, temp3);
        
        SkillIdentify identify4 = new SkillIdentify();
        identify4.keyCode = KeyCode.E;
        identify4.skillId = Define.SkillId.DaggerBall;
        ActiveSkill temp4 = GI.Inst.ResourceManager.GetSkillDataCopy(Define.SkillId.DaggerBall) as ActiveSkill;
        temp4.Init(1);
        AvailableSkills.Add(identify4, temp4);
        
    }

    void ExecuteSkill(int instanceId, Define.SkillId skillId)
    {
        if (Player.InstId != instanceId) return;
      
        foreach (var pair in AvailableSkills)
        {
            if (pair.Key.skillId == skillId)
            {
                pair.Value.ExecuteSkill(Player.StatManager, Player.arrowSpawnPoint, Player.PlayerController);
            }
        }
    }

    public ActiveSkill GetSkill(KeyCode keyCode)
    {
        foreach (var pair in AvailableSkills)
        {
            if (pair.Key.keyCode == keyCode)
                return pair.Value;
        }
        return null;
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
            
            GameObject daggerObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.PlayerClone, trans.position, quaternion.identity);
            SkillAbility_PlayerClone playerClone = daggerObj.GetComponent<SkillAbility_PlayerClone>();
            playerClone.Init(castStatManager, damageInfo, enemyStatManager);
            yield return new WaitForSeconds(0.2f);
        }
    }
    

    #endregion
    
    
}
