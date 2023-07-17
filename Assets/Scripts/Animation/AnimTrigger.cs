using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    protected Animator Animator { get; set; }
    
    protected CombatManager CombatManager { get; set; }

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        
        CombatManager = GetComponentInParent<CombatManager>();
    }
    
    public virtual void GoToIdleState()
    {
    }

    public void OnSpawnArrow()
    {
        CombatManager.BowPrimaryAttack(); 
    }

    public virtual void OnExecSkillTrigger()
    {
    }
    
    public virtual void PauseAnimation()
    {
    }

    public void ResumeAnimation()
    {
        Animator.speed = 1f;
    }
    
    public virtual void SweepOverlapCircle()
    {
    }

    public virtual void ExecNormalAttackNotify()
    {
    }

    public virtual void NormalAttackCompleted()
    {
        
    }
    
    
}
