using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] public List<string> haveItems = new List<string>();

    public List<string> GetItemIds()
    {
        return haveItems;
    }
    
}
