using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private Image background;
    
    
    public void FadeIn(Action callback)
    {
        StartCoroutine(CoFadeIn(callback));
    }

    IEnumerator CoFadeIn(Action callback)
    {
        Color original = background.color;
        float a;
        
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
        StartCoroutine(CoFadeOut(callback));
    }
    
    IEnumerator CoFadeOut(Action callback)
    {
        Color original = background.color;
        float a;
        
        while (true)
        {
            a = Mathf.Lerp(background.color.a, 255f, Time.deltaTime);
            background.color = new Color(original.r, original.g, original.b, a);
            yield return null;
            if (a <= 0f) break;
        }
        callback?.Invoke();
    }
}
