using System;


public static class Define
{
    public const int MAX_NORMAL_ATTACK_COMBO_NUM = 3;
    public const int LEFT_PIVOT = 0;
    public const int RIGHT_PIVOT = 1;

    public enum EBossAttackType
    {
        NormalAttack1,
        NormalAttack2,
        NormalAttack3,
        SpecialAttack1,
        SpecialAttack2,
        SpecialAttack3,
        
    }

    public enum EDamageTextType
    {
        PlayerDamaged,
        MonsterDamaged,
        MonsterDefendDamaged,
        PlayerDamagedCritical,
        MonsterDamagedCritical,
        MonsterDefendDamagedCritical,
        Evasion,
        Dodge
    }

    public enum EActivationCondition //발동 조건
    {
        None,
        CauseDamage,
        TakeDamage
    }
    

    public enum EDamageType
    {
        None,
        Normal,
        Skill,
        Both
    }
    
    
    public enum ELabel
    {
        Prefab,
        Skill,
        Item,
        MonsterInfo,
        PlayerBaseStat,
        ItemCraft,
        PlayerControl,
        AudioClip,
        AudioMixer
    }

    public enum ESkillId
    {
        ThrowDagger, PlayerClone, DaggerBall, DaggerUlt,
        FireStrike, Earthquake, ThrowAxe, AxeUlt,
        ArrowRain, PiercingArrow, ArrowBuff, DistortionArrow,
        Dash,
        HealthSteal, LastStand, TemporalDistortion, DeadlyImpact, NimbleReflexes,
        Max
    }
    

    public enum EStatType
    {
        None,
        Attack,
        Defence,
        ElemAttack,
        MaxHp,
        CriticalChancePer,
        CriticalResistPer,
        CriticalDamageIncPer,
        NormalAttackDamageIncPer,
        SkillAttackDamageIncPer,
        EvasionChancePer,
        SkillCooltimeDecRate,
        MoveSpeed,
        
        Max
    }
    
    public enum EPlayerState
    {
        Idle, Move, InAir, Falling, JumpEnd,
        GroundSliding, WallSliding, WallJump, Dash, 
        DaggerNormalAttack, AxeNormalAttack, BowNormalAttack,
        ThrowDaggerSkill, PlayerCloneSkill, DaggerUlt, DaggerBall,
        FireStrike, Earthquake, ThrowAxe, AxeUlt,
        ArrowRain, PiercingArrow, ArrowBuff, DistortionArrow,
        
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
        NormalAttack1,
        NormalAttack2,
        NormalAttack3,
        SpecialAttack1,
        SpecialAttack2,
        Freeze,
        Dead,
        Dodge,
        Defend,

        Max
    }

    public enum EStatusEffect
    {
        None, Burn, Poison, Freeze, Bleed, Fear,
        
        Max
    }
    
    public enum EMainMenuType
    {
        None,
        Inventory,
        Skill
    }

    public enum EOptionType
    {
        Sound,
        Display,
        BindKey
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
    
    
}
