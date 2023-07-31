using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private Image background;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    
    public void FadeIn(Action callback)
    {
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);
        fadeInCoroutine = StartCoroutine(CoFadeIn(callback));
    }

    IEnumerator CoFadeIn(Action callback)
    {
        Color original = background.color;
        background.color = new Color(original.r, original.g, original.b, 1f);
        float a = 0;
        
        while (true)
        {
            a = Mathf.Lerp(background.color.a, 0f, Time.deltaTime);
            background.color = new Color(original.r, original.g, original.b, a);
            yield return null;
            if (a <= 0f) break;
        }
        callback?.Invoke();
    }
    
    public void FadeOut(Action callback)
    {
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        fadeOutCoroutine = StartCoroutine(CoFadeOut(callback));
    }
    
    IEnumerator CoFadeOut(Action callback)
    {
        Color original = background.color;
        background.color = new Color(original.r, original.g, original.b, 0f);
        float a;
        float targetAlpha = 1f;
        float speed = 1f; 
        
        while (background.color.a < targetAlpha)
        {
            float newAlpha = Mathf.MoveTowards(background.color.a, targetAlpha, speed * Time.deltaTime);
            //a = Mathf.Lerp(background.color.a, 1f, Time.deltaTime);
            background.color = new Color(original.r, original.g, original.b, newAlpha);
            yield return null;
        }
        callback?.Invoke();
    }
}
