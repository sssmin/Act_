

using System;
using UnityEngine;

public static class Define
{
    public const int MAX_NORMAL_ATTACK_COMBO_NUM = 3;
    
    public enum ScriptableObjectType
    {
        Skill,
        Stat
    }
    
    public enum SkillId
    {
        ThrowDagger,
        PlayerClone,
        DaggerBall,
        DaggerUlt
    }

    public enum BaseStatOwnerId
    {
        Player,
        NightBorne
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
    
    public enum EPlayerState
    {
        Idle,
        Move,
        InAir,
        Falling,
        JumpEnd,
        GroundSliding,
        WallSliding,
        WallJump,
        Dash,
        DaggerNormalAttack,
        AxeNormalAttack,
        BowNormalAttack,
        ThrowDaggerSkill,
        PlayerCloneSkill,
        DaggerUlt,
        DaggerBall,
        
        Dead,
        
        Max
    }

    [Serializable]
    public enum EMonsterState
    {
        None,
        Idle,
        Patrol,
        Chase,
        CrowdControl,
        NormalAttack,
        Freeze,
        //todo 공격종류 별로?
        Dead,

        Max
    }

    public enum EStatusEffect
    {
        None,
        Burn,
        Poison,
        Freeze,
        Bleed,
        Fear,
        
        Max
    }
    
}
