using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    protected Rigidbody2D Rb { get; set; }
    public float speed;
    [HideInInspector]
    public BaseController OwnerController;
 
    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ;
    }

    void Update()
    {
        
    }
}
