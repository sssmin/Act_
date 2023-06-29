using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterInfo : MonoBehaviour
{
    [SerializeField] private RectTransform gameObjectTransform;
    [SerializeField] private RectTransform HpBar;
    [SerializeField] private Image statusImage;

    public void SetBar(float ratio)
    {
        float xValue = ratio;
        HpBar.sizeDelta = new Vector2(xValue, HpBar.sizeDelta.y);
    }

    
    public void InitPos()
    {
        gameObjectTransform.localPosition = new Vector3(0f, 2.5f, gameObjectTransform.localPosition.z);
        gameObjectTransform.localScale = new Vector3(0.025f, 0.025f, gameObjectTransform.localScale.z);
    }

    public void Flip(BaseController.EDir dir)
    {
        if (dir == BaseController.EDir.Left)
            gameObjectTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        else 
            gameObjectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
