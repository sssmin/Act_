using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterInfo : MonoBehaviour
{
    [SerializeField] private RectTransform gameObjectTransform;
    [SerializeField] private RectTransform HpBar;
    [SerializeField] private Image statusImage;
    [SerializeField] private Animator animator;
    private Canvas canvas;
    
    private Coroutine iconEndCoroutine;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.sortingLayerName = "Instance";
        canvas.sortingOrder = 1;
    }

    public void SetBar(float ratio)
    {
        float xValue = ratio;
        HpBar.sizeDelta = new Vector2(xValue, HpBar.sizeDelta.y);
    }

    
    public void InitPos(float spawnXPos, float spawnYPos)
    {
        gameObjectTransform.localPosition = new Vector3(spawnXPos, spawnYPos, gameObjectTransform.localPosition.z);
        gameObjectTransform.localScale = new Vector3(0.025f, 0.025f, gameObjectTransform.localScale.z);
    }

    public void Flip(BaseController.EDir dir)
    {
        if (dir == BaseController.EDir.Left)
            gameObjectTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        else 
            gameObjectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void SetStatusImage(Sprite icon, float endTime)
    {
        statusImage.color = Color.white;
        statusImage.sprite = icon;
        if (iconEndCoroutine != null) StopCoroutine(iconEndCoroutine);
        float duration = endTime - Time.time;
        iconEndCoroutine = StartCoroutine(CoSetStatusImage(duration));
    }

    private IEnumerator CoSetStatusImage(float second)
    {
        float blinkTime = second - 3f; //3초가 남았을 때 깜빡임
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(blinkTime);
        animator.SetBool("isIdle", false);
        animator.SetBool("isBlink", true);
        yield return new WaitForSeconds(3f);
        animator.SetBool("isBlink", false);
        statusImage.sprite = null;
        statusImage.color = Color.clear;
    }
    
    
    
}
