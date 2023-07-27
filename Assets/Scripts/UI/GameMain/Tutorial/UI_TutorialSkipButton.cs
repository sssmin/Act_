using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialSkipButton : MonoBehaviour
{
    [SerializeField] private Button skipButton;
    void Start()
    {
        skipButton.onClick.RemoveListener(OnClickSkipButton);
        skipButton.onClick.AddListener(OnClickSkipButton);
    }

    private void OnDestroy()
    {
        skipButton.onClick.RemoveListener(OnClickSkipButton);
    }

    private void OnClickSkipButton()
    {
        GI.Inst.TutorialManager.SkipCurrentTutorial();
    }
}
