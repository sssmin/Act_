using System.Collections;
using TMPro;
using UnityEngine;

public class UI_TutorialTitle : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI titleText;
    
    private void Start()
    {
        titleText.color = Color.clear;
    }

    public void SetTitle(string text)
    {
        titleText.text = text;
    }

    public void BeginAnimCompletedNotify()
    {
        animator.SetBool(AnimHash.isIdle, true);
        StartCoroutine(CoStartDestroyAnim(2f));
    }

    IEnumerator CoStartDestroyAnim(float second)
    {
        yield return new WaitForSeconds(second);
        animator.SetBool(AnimHash.isIdle, false);
        animator.SetBool("isDestroy", true);
    }

    public void DestroyAnimCompletedNotify()
    {
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
