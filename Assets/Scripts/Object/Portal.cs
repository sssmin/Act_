using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
        {
            GI.Inst.UIManager.CreateDungeonInfoUI();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            GI.Inst.UIManager.DestroyDungeonSelectUI();
        }
    }
}
