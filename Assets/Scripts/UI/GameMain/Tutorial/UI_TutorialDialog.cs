using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ETutorialStatus
{
    Blank,
    InProgress,
    Complete
}

public class UI_TutorialDialog : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI tutorialText; //튜토리얼 설명
    [SerializeField] private TextMeshProUGUI statusText; //튜토리얼 시작(빈칸), 진행중, 완료

    private float typingSpeed = 0.1f;
    private bool isTypingEffect;
    private string dialogText;
    private Coroutine updateDialogCoroutine;
    private TutorialStep tutorialStepDialog;
    private EDialogType dialogType;
    [SerializeField] List<AudioClip> typingSounds = new List<AudioClip>();
    

    public void Init(string text, float x, float y, TutorialStep step, EDialogType type)
    {
        SetStatusText(ETutorialStatus.Blank);
        dialogType = type;
        rectTransform.SetLocalPositionAndRotation(new Vector3(x, y, 0), quaternion.identity);
        FadeIn();
        dialogText = text;
        tutorialStepDialog = step;
        
        if (updateDialogCoroutine != null)
            StopCoroutine(updateDialogCoroutine);
        updateDialogCoroutine = StartCoroutine(CoUpdateDialog());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (isTypingEffect)
            {
                tutorialText.text = dialogText;
                if (updateDialogCoroutine != null)
                    StopCoroutine(updateDialogCoroutine);
            
                isTypingEffect = false;
                if (dialogType == EDialogType.TypingAndWaitAction)
                    SetStatusText(ETutorialStatus.InProgress, tutorialStepDialog);
                else if (dialogType == EDialogType.TypingAndNextStep)
                    StartCoroutine(CoSetNextTutorialStep(1.5f, tutorialStepDialog));
                
            }
        }
    }

    IEnumerator CoUpdateDialog()
    {
        isTypingEffect = true;
        StartCoroutine(CoPlayTypingEffectSound());
        for (int i = 0; i < dialogText.Length; i++)
        {
            tutorialText.text = dialogText.Substring(0, i);

            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTypingEffect = false;
        
        if (dialogType == EDialogType.TypingAndWaitAction)
            SetStatusText(ETutorialStatus.InProgress, tutorialStepDialog);
        else if (dialogType == EDialogType.TypingAndNextStep)
            StartCoroutine(CoSetNextTutorialStep(1.5f, tutorialStepDialog));
        
    }

    IEnumerator CoPlayTypingEffectSound()
    {
        while (isTypingEffect)
        {
            float length = GI.Inst.SoundManager.PlayEffectSound(typingSounds[Random.Range(0, typingSounds.Count)]);
            yield return new WaitForSeconds(length + 0.02f);
        }
    }

    public void SetStatusText(ETutorialStatus tutorialStatus)
    {
        switch (tutorialStatus)
        {
            case ETutorialStatus.Blank:
                statusText.text = "";
                break;
            case ETutorialStatus.InProgress:
                statusText.text = "- 진행 중";
                break;
            case ETutorialStatus.Complete:
                statusText.text = "- 완료";
                break;
        }
    }
    
    public void SetStatusText(ETutorialStatus tutorialStatus, TutorialStep step)
    {
        StartCoroutine(CoSetStatusText(tutorialStatus, step));
    }

    IEnumerator CoSetStatusText(ETutorialStatus tutorialStatus, TutorialStep step)
    {
        switch (tutorialStatus)
        {
            case ETutorialStatus.Blank:
                statusText.text = "";
                break;
            case ETutorialStatus.InProgress:
                statusText.text = "- 진행 중";
                break;
            case ETutorialStatus.Complete:
                statusText.text = "- 완료";
                break;
        }

        yield return new WaitForSeconds(0.5f);
        step.IsCompleted = true;
    }

    IEnumerator CoSetNextTutorialStep(float second, TutorialStep step)
    {
        yield return new WaitForSeconds(second);
        step.IsCompleted = true;
    }

    public void Clear()
    {
        tutorialText.text = "";
        statusText.text = "";
    }

    public void Visible()
    {
        gameObject.SetActive(true);
    }

    public void Invisible()
    {
        gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        float a = 0f;
        while (a <= 0.9f)
        {
            a = Mathf.Lerp(tutorialText.color.a, 1f, Time.deltaTime * 3f);
            tutorialText.color = new Color(tutorialText.color.r, tutorialText.color.g, tutorialText.color.b, a);
            statusText.color = new Color(statusText.color.r, statusText.color.g, statusText.color.b, a);
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, a);
            yield return null;
        }
    }

    public void FadeOut(TutorialStep step)
    {
        StartCoroutine(CoFadeOut(step));
    }

    IEnumerator CoFadeOut(TutorialStep step)
    {
        float a = 1f;
        while (a >= 0.1f)
        {
            a = Mathf.Lerp(tutorialText.color.a, 0f, Time.deltaTime * 3f);
            tutorialText.color = new Color(tutorialText.color.r, tutorialText.color.g, tutorialText.color.b, a);
            statusText.color = new Color(statusText.color.r, statusText.color.g, statusText.color.b, a);
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, a);
            yield return null;
        }

        step.IsCompleted = true;
    }
}
