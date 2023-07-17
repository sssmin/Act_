using UnityEngine;
using UnityEngine.UI;

public class UI_Main_SkillHotkeySlot : UI_Main_SkillHotkeySlotBase
{
    [SerializeField] private Image cooltimeImage;
    [SerializeField] private ParticleSystem temporalDistortionParticle;
    [SerializeField] private Animator animator;
    
    private float cooltime = -1f;
    
    private void Update()
    {
        if (cooltime >= 0f) 
        {
            cooltimeImage.fillAmount -= (1 / cooltime * Time.deltaTime);
            if (cooltimeImage.fillAmount <= 0)
                cooltime = -1f;
        }
    }

    public override void SetSkillIcon(Sprite icon)
    {
        base.SetSkillIcon(icon);
        
        cooltimeImage.color = new Color(36f / 255f, 36 / 255f, 36 / 255f, 163f / 255f);
    }

    public override void Clear()
    {
        base.Clear();
        
        cooltimeImage.color = Color.clear;
    }

    public void SetCooltime(float inCooltime)
    {
        cooltime = inCooltime;
        cooltimeImage.fillAmount = 1;
    }

    public void ResetCooltimeUI()
    {
        cooltime = -1f;
        cooltimeImage.fillAmount = 0;
        PlayTemporalDistortionParticle();
        animator.SetBool(AnimHash.temporal, true);
    }

    private void PlayTemporalDistortionParticle()
    {
        temporalDistortionParticle.Play();
    }

    public void AnimCompletedNotify()
    {
        animator.SetBool(AnimHash.temporal, false);
    }
}
