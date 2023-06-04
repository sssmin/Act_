using UnityEngine;

    public static class AnimHash
    {
        /*
         * common
         */
        public static readonly int isIdle = Animator.StringToHash("isIdle");
        public static readonly int isMove = Animator.StringToHash("isMove");
        public static readonly int xVelocity = Animator.StringToHash("xVelocity");
        public static readonly int yVelocity = Animator.StringToHash("yVelocity");
        
        /*
         * player
         */
        public static readonly int isInAir = Animator.StringToHash("isInAir");
        public static readonly int isDash = Animator.StringToHash("isDash");
        public static readonly int isJumpEnd = Animator.StringToHash("isJumpEnd");
        public static readonly int isFalling = Animator.StringToHash("isFalling");
        public static readonly int groundSlide = Animator.StringToHash("groundSlide");
        public static readonly int wallSlide = Animator.StringToHash("wallSlide");
        public static readonly int attackComboNum = Animator.StringToHash("attackComboNum");
        public static readonly int isDaggerNormalAttack = Animator.StringToHash("isDaggerNormalAttack");
        public static readonly int isAxeNormalAttack = Animator.StringToHash("isAxeNormalAttack");
        public static readonly int isBowNormalAttack = Animator.StringToHash("isBowNormalAttack");
        public static readonly int isThrowDaggerSkill = Animator.StringToHash("isThrowDaggerSkill");
        public static readonly int isDaggerCloneSkill = Animator.StringToHash("isDaggerCloneSkill");
        public static readonly int isDaggerUltBegin = Animator.StringToHash("isDaggerUltBegin");
        public static readonly int daggerUltNum = Animator.StringToHash("daggerUltNum");
        public static readonly int isDaggerBallSkill = Animator.StringToHash("isDaggerBallSkill");
        /*
         * monster
         */
        
        
        /*
         * object
         */
        public static readonly int daggerExplosionTrigger = Animator.StringToHash("daggerExplosionTrigger");
        public static readonly int isDaggerUltExplosion = Animator.StringToHash("isDaggerUltExplosion");
        public static readonly int daggerBallDestroy = Animator.StringToHash("daggerBallDestroy");
        
        
    }
