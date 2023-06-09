

using System;
using UnityEngine;

public static class Define
{
    public const int MAX_NORMAL_ATTACK_COMBO_NUM = 3;

    public enum EActivationCondition //발동 조건
    {
        ApplyImmediately,
        CauseDamage,
        TakeDamage
    }

    public enum EEffectType
    {
        Buff,
        Debuff,
        Attr
    }

    public enum EDamageType
    {
        None,
        Normal,
        Skill
    }

    public enum EEffectDetail
    {
        None,
        GetValue,
        ReturnIncDecValue,
        SpawnInstance,
        BuffMySelf,
        DebuffToEnemy
    }
    
    public enum EScriptableObjectType
    {
        Skill,
        Stat
    }

    public enum EBaseStatOwnerId
    {
        Player,
        NightBorne
    }

    // public enum EBuffType
    // {
    //     PassiveBuff, //지속 버프
    //     ActiveBuff   //단발성 버프
    // }
    
    
    //쭉 지속되는 버프인지
    //Endtime이 있는 버프인지

    public enum EBuffType
    {
        StatBuff, //스탯 버프
        EnhancementBuff //강화 버프
    }

    public enum EBuffId
    {
        ArrowBuff,
    }

    public enum EDebuffId
    {
        
    }

    public enum EAttrId
    {
        
    }

    public enum EItemId
    {
        
    }

    public enum ESkillId
    {
        ThrowDagger,
        PlayerClone,
        DaggerBall,
        DaggerUlt,
        FireStrike,
        Earthquake,
        ThrowAxe,
        AxeUlt,
        ArrowRain,
        PiercingArrow,
        ArrowBuff,
        DistortionArrow,
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
        FireStrike,
        Earthquake,
        ThrowAxe,
        AxeUlt,
        ArrowRain,
        PiercingArrow,
        ArrowBuff,
        DistortionArrow,
        
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
        Suppression,
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
    
}
