
public class PlayerAnimTrigger : AnimTrigger
{
    private Player Player { get; set; }

    public override void Awake()
    {
        base.Awake();
        
        Player = GetComponentInParent<Player>();
    }


    public override void GoToIdleState()
    {
        if (Player)
            Player.TransitionState(Define.EPlayerState.Idle);
    }
    
    public override void OnExecSkillTrigger()
    {
        if (Player)
            GI.Inst.ListenerManager.OnTriggerAnim(Player.InstId);
    }
    
    public override void PauseAnimation()
    {
        Animator.speed = 0f;
        Player.AnimPauseNotify();//State에 Trigger
    }
    
    public void DaggerUltCastingStart()
    {
        if (Player)
            GI.Inst.ListenerManager.OnTriggerAnim(Player.InstId);
    }
    
    public override void SweepOverlapCircle()
    {
        CombatManager.ExecuteNormalAttack("Monster");
    }
    
}
