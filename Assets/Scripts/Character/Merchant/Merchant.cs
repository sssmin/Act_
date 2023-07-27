using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] public List<string> haveItems = new List<string>();
    private BoxCollider2D VisibleUICollider { get; set; }
    private Canvas InteractUI { get; set; }

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
            InteractUI.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            InteractUI.gameObject.SetActive(false);
    }
}
