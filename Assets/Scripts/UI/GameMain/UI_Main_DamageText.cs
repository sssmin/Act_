using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Main_DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private RectTransform rectTransform;
    
    private void OnEnable()
    {
        StartCoroutine(CoDestroyMyself());
    }

    private void Update()
    {
        float a = Mathf.Lerp( damageText.color.a, 0f, Time.deltaTime);
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, a);
    }

    public void Init(Define.EDamageTextType damageTextType, float damage)
    {
        string assembleDamage = "";
        switch (damageTextType)
        {
            case Define.EDamageTextType.PlayerDamaged:
                damageText.color = Color.red;
                break;
            case Define.EDamageTextType.MonsterDamaged:
                damageText.color = new Color(255f/255f, 157f/255f, 0f, 255f/255f);
                break;
            case Define.EDamageTextType.MonsterDefendDamaged:
                damageText.color = new Color(183f/255f, 183f/255f, 183f/255f, 255f/255f);
                assembleDamage = "Defend!\n";
                break;
            case Define.EDamageTextType.PlayerDamagedCritical:
                damageText.color = Color.red;
                assembleDamage = "Critical!\n";
                break;
            case Define.EDamageTextType.MonsterDamagedCritical:
                damageText.color = new Color(255f/255f, 157f/255f, 0f, 255f/255f);
                assembleDamage = "Critical!\n";
                break;
            case Define.EDamageTextType.MonsterDefendDamagedCritical:
                damageText.color = new Color(183f/255f, 183f/255f, 183f/255f, 255f/255f);
                assembleDamage = "Defend!\n";
                break;
            case Define.EDamageTextType.Evasion:
                damageText.color = new Color(183f/255f, 183f/255f, 183f/255f, 255f/255f);
                assembleDamage = "Evasion!\n";
                damageText.text = assembleDamage;
                return;
            case Define.EDamageTextType.Heal:
                damageText.color = Color.green;
                damageText.text = assembleDamage;
                break;
            case Define.EDamageTextType.Dodge:
                damageText.color = new Color(183f/255f, 183f/255f, 183f/255f, 255f/255f);
                assembleDamage = "Dodge!\n";
                damageText.text = assembleDamage;
                return;
        }
        damageText.text = assembleDamage + damage.ToString("#,0");
    }

    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(2f);
        GI.Inst.ResourceManager.Destroy(transform.parent.gameObject);
    }
    
}
