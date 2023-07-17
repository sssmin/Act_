using UnityEngine;



public class SO_PassiveSkill : SO_Skill
{
    [HideInInspector] public Effect effect;
    [HideInInspector] public int equipIndex = -1;

    public virtual void Init()
    {
        
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        
    }
}
