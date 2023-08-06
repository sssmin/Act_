using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
   
    private Animator Animator { get; set; }
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    private Action Callback { get; set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void FadeIn(Action callback)
    {
        Callback = callback;
        Animator.SetBool("fadeOut", false);
        Animator.SetBool("fadeIn", true);
    }
    
    
    public void FadeOut(Action callback)
    {
        Callback = callback;
        Animator.SetBool("fadeIn", false);
        Animator.SetBool("fadeOut", true);
    }
    public void FadeInCompleteNotify()
    {
        Callback?.Invoke();
        Animator.SetBool("fadeIn", false);
    }


    public void FadeOutCompleteNotify()
    {
        Callback?.Invoke();
        Animator.SetBool("fadeOut", false);
    }
}
   
