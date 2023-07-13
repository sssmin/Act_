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
        newGameButton.onClick.AddListener(OnClickNewGameButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickNewGameButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.SceneLoadManager.OnClickNewGameButton("Tutorial");
    }
    
    private void OnClickContinueButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.SceneLoadManager.OnClickContinueGameButton();
    }
    
    private void OnClickOptionButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.VisibleOption(Define.EOptionType.Sound);
    }
    
    private void OnClickExitButton()
    {
        Application.Quit();
    }
}
