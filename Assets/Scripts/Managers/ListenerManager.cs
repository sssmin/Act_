using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerManager
{
    public Action<int, Define.ESkillId> onExecuteSkill;
    public Action<int, Define.EPlayerState> onTransitionStateReq;
    public Func<KeyCode, ActiveSkill> getSkill;
    public Action<int> onTriggerAnim;
    public Action<int, StatManager, DamageInfo> onExecPlayerClone;
    public Action<int, StatManager, DamageInfo> onExecEarthquake;
    
    public void OnExecuteSkill(int instanceId, Define.ESkillId skillId)
    {
        onExecuteSkill?.Invoke(instanceId, skillId);
    }

    public void OnTransitionStateReq(int instanceId, Define.EPlayerState playerState)
    {
        onTransitionStateReq?.Invoke(instanceId, playerState);
    }

    public ActiveSkill GetSkill(KeyCode keyCode)
    {
        return getSkill?.Invoke(keyCode);
    }

    public void OnTriggerAnim(int instanceId)
    {
        onTriggerAnim?.Invoke(instanceId);
    }

    public void OnExecPlayerClone(int instanceId, StatManager castStatManager, DamageInfo damageInfo)
    {
        onExecPlayerClone?.Invoke(instanceId, castStatManager, damageInfo);
    }
    
    public void OnExecEarthquake(int instanceId, StatManager castStatManager, DamageInfo damageInfo)
    {
        onExecEarthquake?.Invoke(instanceId, castStatManager, damageInfo);
    }
   





}
