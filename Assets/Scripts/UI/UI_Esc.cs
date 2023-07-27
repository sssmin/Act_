using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Esc : UI_Popup
{
    [SerializeField] private Button optionButton;
    [SerializeField] private Button saveGameButton;
    [SerializeField] private TextMeshProUGUI saveText;
    [SerializeField] private Button goToTitleButton;
    [SerializeField] private Button gameExitButton;
    [SerializeField] private Button closeButton;
    
    private void Start()
    {
        RemoveListener();
        optionButton.onClick.AddListener(OnClickOptionButton);
        saveGameButton.onClick.AddListener(OnClickSaveGameButton);
        goToTitleButton.onClick.AddListener(OnClickGoToTitleButton);
        gameExitButton.onClick.AddListener(OnClickGameExitButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(optionButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(saveGameButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(goToTitleButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(gameExitButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(closeButton);
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void RemoveListener()
    {
        optionButton.onClick.RemoveListener(OnClickOptionButton);
        saveGameButton.onClick.RemoveListener(OnClickSaveGameButton);
        goToTitleButton.onClick.RemoveListener(OnClickGoToTitleButton);
        gameExitButton.onClick.RemoveListener(OnClickGameExitButton);
        closeButton.onClick.RemoveListener(OnClickCloseButton);
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Town")
        {
            saveGameButton.interactable = false;
            saveText.color = new Color(224f / 255f, 224f / 255f, 224f / 255f, 100f / 255f);
        }
        else
        {
            saveGameButton.interactable = true;
            saveText.color = new Color(224f / 255f, 224f / 255f, 224f / 255f, 255f / 255f);
        }
    }

    private void OnClickOptionButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.VisibleOption(Define.EOptionType.Sound, false);
    }
    
    private void OnClickSaveGameButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.SaveGame();
    }
    
    private void OnClickGoToTitleButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.InvisibleEsc(false);
        GI.Inst.SceneLoadManager.GoToTitle();
    }
    
    private void OnClickGameExitButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        Application.Quit();
    }
    
    private void OnClickCloseButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.InvisibleEsc(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
