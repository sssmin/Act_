using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] public List<string> haveItems = new List<string>();
    private BoxCollider2D VisibleUICollider { get; set; }
    private Canvas InteractUI { get; set; }
    [SerializeField] private TextMeshProUGUI interactText;

    private void Awake()
    {
        VisibleUICollider = GetComponent<BoxCollider2D>();
        InteractUI = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        InteractUI.gameObject.SetActive(false);
    }

    public List<string> GetItemIds() => haveItems;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
        {
            InteractUI.gameObject.SetActive(true);
            string keyString = GI.Inst.Player.PlayerController.GetBindingKeyString(EBindKeyType.Interaction);
            interactText.text = $"{keyString} - 상호작용";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            InteractUI.gameObject.SetActive(false);
    }
}
