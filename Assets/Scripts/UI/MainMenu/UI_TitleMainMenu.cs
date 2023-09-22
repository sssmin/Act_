using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleMainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;

    public void Awake()
    {
        RemoveListener();
        newGameButton.onClick.AddListener(OnClickNewGameButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(continueButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(newGameButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(optionButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(exitButton);
        if (!GI.Inst.IsExistSaveData())
            continueButton.interactable = false;
        
    }

    private void RemoveListener()
    {
        newGameButton.onClick.RemoveListener(OnClickNewGameButton);
        continueButton.onClick.RemoveListener(OnClickContinueButton);
        optionButton.onClick.RemoveListener(OnClickOptionButton);
        exitButton.onClick.RemoveListener(OnClickExitButton);
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void OnClickNewGameButton()
    {
        GI.Inst.SoundManager.PlayEffectSound("ButtonClick");
        Destroy(gameObject);
        GI.Inst.SceneLoadManager.OnClickNewGameButton("Tutorial");
    }
    
    private void OnClickContinueButton()
    {
        GI.Inst.SoundManager.PlayEffectSound("ButtonClick");
        Destroy(gameObject);
        GI.Inst.SceneLoadManager.OnClickContinueGameButton();
    }
    
    private void OnClickOptionButton()
    {
        GI.Inst.SoundManager.PlayEffectSound("ButtonClick");
        GI.Inst.UIManager.VisibleOption(Define.EOptionType.Sound, true);
    }
    
    private void OnClickExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
