using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] RectTransform contentWrapper; //배치는 얘로
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescText;
    [SerializeField] private TextMeshProUGUI skillCooltimeText;

    private void Awake()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }
    
    public void Init(ActiveSkill_ShortVer skill, Vector3 slotPos, int pivot)
    {
        InitSize(slotPos, pivot);
        skillIconImage.sprite = skill.icon;
        skillLevelText.text = $"Lv. {skill.level}";
        skillNameText.text = skill.skillName;
        skillDescText.text = skill.skillDesc;
        if (skill.skillCooltime > 0f)
            skillCooltimeText.text = $"쿨타임 {skill.skillCooltime:#,0} 초";
        else
            skillCooltimeText.text = "";
    }
    
    public void Init(PassiveSkill_ShortVer skill, Vector3 slotPos, int pivot)
    {
        InitSize(slotPos, pivot);
        skillIconImage.sprite = skill.icon;
        skillLevelText.text = $"Lv. {skill.level}";
        skillNameText.text = skill.skillName;
        skillDescText.text = skill.skillDesc;
        if (skill.skillCooltime > 0f)
            skillCooltimeText.text = $"쿨타임 {skill.skillCooltime:#,0} 초";
        else
            skillCooltimeText.text = "";
    }
    
    private void InitSize(Vector3 slotPos, int pivot)
    {
        contentWrapper.pivot = new Vector2(pivot, 0.5f);
        contentWrapper.transform.position = slotPos;
    }
}
