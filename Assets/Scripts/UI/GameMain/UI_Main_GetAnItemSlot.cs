using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_GetAnItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        itemIcon.sprite = null;
        itemNameText.text = "";
        itemAmountText.text = "";
        
        animator.SetBool("isFadeIn", true);
    }

    public void SetInfo(Sprite icon, string itemName, string amount)
    {
        itemIcon.sprite = icon;
        itemNameText.text = itemName;
        itemAmountText.text = $"x{amount}";
    }

    public void SetGoldInfo(Sprite icon, string gold)
    {
        itemIcon.sprite = icon;
        itemNameText.text = "골드";
        itemAmountText.text =  $"x{gold}";
    }

    public void FadeInCompletedNotify()
    {
        animator.SetBool("isFadeIn", false);
        StartCoroutine(CoFadeOut(1.5f));
    }

    IEnumerator CoFadeOut(float second)
    {
        yield return new WaitForSeconds(second);
        
        //fadeout 시작
        animator.SetBool("isFadeOut", true);
    }

    public void FadeOutCompletedNotify()
    {
        animator.SetBool("isFadeOut", false);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
