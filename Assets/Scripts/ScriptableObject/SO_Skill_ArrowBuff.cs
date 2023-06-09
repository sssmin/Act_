using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/ArrowBuff")]
public class SO_Skill_ArrowBuff : ActiveSkill
{
    public Effect effect;
    
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject arrowBuffObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.ArrowBuff, playerController.transform.position, Quaternion.identity);
        SkillAbility_ArrowBuff skillAbilityArrowBuff = arrowBuffObj.GetComponent<SkillAbility_ArrowBuff>();

        int perDmg = skillLevel * 2;
        skillAbilityArrowBuff.Init(effect, castStatManager, perDmg);
    }

    
}