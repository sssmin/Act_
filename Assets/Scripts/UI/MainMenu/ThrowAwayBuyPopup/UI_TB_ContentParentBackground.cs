using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TB_ContentParentBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button backgroundButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundButton.interactable = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundButton.interactable = true;
    }
}
