using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TB_ContentParentBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button backgroundButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundButton.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundButton.enabled = true;
    }
}
