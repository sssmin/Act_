using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseCharacter : MonoBehaviour
{
    private int instId;
    public int InstId
    {
        get => instId;
        private set => instId = value;
    }
    protected Rigidbody2D Rb { get; set; }
    protected SpriteRenderer Sr { get; set; }
    public Animator Animator { get; set; }
    public StateMachine StateMachine { get; set; }
    
    public float MoveSpeed { get; protected set; }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<Animator>();
        StateMachine = GetComponent<StateMachine>();
        InstId = GetInstanceID();
    }

    protected virtual void Start()
    {
        InitState();
    }

    protected virtual void Update()
    {
        StateMachine.CurrentState.Update();
    }

    protected virtual void InitState()
    {
    }
    
    public void SetZeroVelocity()
    {
        Rb.velocity = new Vector2(0f, 0f);
    }

    public void SetVelocity(float xValue)
    {
        Rb.velocity = new Vector2(xValue, Rb.velocity.y);
    }
    
    public void SetVelocity(float xValue, float yValue)
    {
        Rb.velocity = new Vector2(xValue, yValue);
    }
    
    public virtual bool DoNotFlipState()
    {
        return false;
    }
    
    
    
}
