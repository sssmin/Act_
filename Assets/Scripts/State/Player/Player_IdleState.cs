using UnityEngine;

public class Player_IdleState : PlayerState
{
    public Player_IdleState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        //공격 애니메이션이 끝나면 트리거로 Idle 상태가 되는데 공격버튼을 계속 연타하면 Idle로 들어오지 않음.
        if (PlayerController.IsReserveNormalAttack)
        {
            if (GI.Inst.ListenerManager.GetEquippedWeaponType() == SO_Item.EWeaponType.Dagger)
                TransitionState(Define.EPlayerState.DaggerNormalAttack);
            else if (GI.Inst.ListenerManager.GetEquippedWeaponType() == SO_Item.EWeaponType.Axe)
                TransitionState(Define.EPlayerState.AxeNormalAttack);
            else if (GI.Inst.ListenerManager.GetEquippedWeaponType() == SO_Item.EWeaponType.Bow)
                TransitionState(Define.EPlayerState.BowNormalAttack);
        }

        Animator.SetBool(AnimHash.isIdle, true);
        Animator.SetFloat(AnimHash.xVelocity, PlayerController.MoveDir.x);
        Player.SetZeroVelocity();
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isIdle, false);
    }
}
