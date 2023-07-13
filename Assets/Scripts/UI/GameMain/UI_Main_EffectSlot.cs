using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Main_EffectSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooltimeImage;

    private EDurationEffectId effectId;
    private Define.ESkillId skillId;
    
    public void Update()
    {
        if (effectId == EDurationEffectId.None)
        {
            cooltimeImage.fillAmount = GI.Inst.CooltimeManager.GetPassiveFillAmount(skillId);
        }
        else
        {
            cooltimeImage.fillAmount = GI.Inst.Player.StatManager.GetFillAmount(effectId);
        }
        
        if (cooltimeImage.fillAmount <= 0f)
        {
            effectId = EDurationEffectId.None;
            skillId = Define.ESkillId.Max;
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    public void InitDurationEffect(EDurationEffectId inEffectId, Sprite icon)
    {
        effectId = inEffectId;
        iconImage.sprite = icon;
    }
    
    public void InitPassiveSkill(Define.ESkillId inSkillId, Sprite icon)
    {
        skillId = inSkillId;
        iconImage.sprite = icon;
    }
}
