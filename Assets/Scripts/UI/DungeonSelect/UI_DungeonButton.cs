using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DungeonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public EDungeonType dungeonType;
    [SerializeField] public Button button;
    [SerializeField] public TextMeshProUGUI dungeonNameText;
    private bool isDisable;
    private UI_DungeonTooltip dungeonTooltipUI;

    public void InitOnce(EDungeonType type)
    {
        dungeonType = type;
        dungeonNameText.text = SO_DungeonInfo.GetDungeonName(dungeonType);
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(button);

        button.onClick.RemoveListener(OnClickDungeonButton);
        button.onClick.AddListener(OnClickDungeonButton);

        if (!GI.Inst.ResourceManager.IsPrevDungeonCompleted(type))
        {
            button.interactable = false;
            isDisable = true;
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClickDungeonButton);
    }

    private void OnClickDungeonButton()
    {
        GI.Inst.UIManager.OnClickDungeonButton(dungeonType);
        button.interactable = false;
    }

    public void EnableButton()
    {
        button.interactable = true;
    }

    public void DisableButton()
    {
        button.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //isDisable 을가지고, isDisable이게 true면 툴팁 보이게.
        if (isDisable)
        {
            dungeonTooltipUI = GI.Inst.UIManager.GetDungeonTooltipUI();
            dungeonTooltipUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDisable)
        {
            dungeonTooltipUI = GI.Inst.UIManager.GetDungeonTooltipUI();
            dungeonTooltipUI.gameObject.SetActive(false);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isDisable)
        {
            dungeonTooltipUI = GI.Inst.UIManager.GetDungeonTooltipUI();
            if (dungeonTooltipUI)
            {
                Vector2 pos = new Vector2(eventData.position.x - 10f, eventData.position.y);
                dungeonTooltipUI.transform.position = pos;
            }
        }
    }
}
