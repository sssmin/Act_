using UnityEngine;

    public static class AnimHash
    {
        /*
         * common
         */
        public static readonly int isIdle = Animator.StringToHash("isIdle");
        public static readonly int isMove = Animator.StringToHash("isMove");
        public static readonly int isDead = Animator.StringToHash("isDead");
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
        public static readonly int isFireStrike = Animator.StringToHash("isFireStrike");
        public static readonly int isEarthquake = Animator.StringToHash("isEarthquake");
        public static readonly int isThrowAxeSkill = Animator.StringToHash("isThrowAxeSkill");
        public static readonly int isAxeUlt = Animator.StringToHash("isAxeUlt");
        public static readonly int isAxeUltCasting = Animator.StringToHash("isAxeUltCasting");
        public static readonly int isArrowRain = Animator.StringToHash("isArrowRain");
        public static readonly int isPiercingArrowStart = Animator.StringToHash("isPiercingArrowStart");
        public static readonly int isPiercingArrowShoot = Animator.StringToHash("isPiercingArrowShoot");
        public static readonly int isDistortionArrow = Animator.StringToHash("isDistortionArrow");
        /*
         * monster
         */
        public static readonly int isNormalAttack1 = Animator.StringToHash("isNormalAttack1");
        public static readonly int isNormalAttack2 = Animator.StringToHash("isNormalAttack2");
        public static readonly int isNormalAttack3 = Animator.StringToHash("isNormalAttack3");
        public static readonly int isSpecialAttack1 = Animator.StringToHash("isSpecialAttack1");
        public static readonly int isSpecialAttack2 = Animator.StringToHash("isSpecialAttack2");
        public static readonly int isSpecialAttack3 = Animator.StringToHash("isSpecialAttack3");
        public static readonly int isWalk = Animator.StringToHash("isWalk");
        public static readonly int isRun = Animator.StringToHash("isRun");
        public static readonly int isDodge = Animator.StringToHash("isDodge");
        public static readonly int isDefend = Animator.StringToHash("isDefend");
        
        
        /*
         * object
         */
        public static readonly int daggerExplosionTrg = Animator.StringToHash("daggerExplosionTrg");
        public static readonly int isDaggerUltExplosion = Animator.StringToHash("isDaggerUltExplosion");
        public static readonly int daggerBallDestroyTrg = Animator.StringToHash("daggerBallDestroyTrg");
        public static readonly int daggerBallExplosionTrg = Animator.StringToHash("daggerBallExplosionTrg");
        public static readonly int activeFireStrike = Animator.StringToHash("activeFireStrike");
        public static readonly int activeEarthquake = Animator.StringToHash("activeEarthquake");
        public static readonly int activeDistortionArrow = Animator.StringToHash("activeDistortionArrow");
        
        /*
         * ui
         */
        public static readonly int blink = Animator.StringToHash("blink");
        public static readonly int temporal = Animator.StringToHash("temporal");
        
        
    }
