using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseController : MonoBehaviour
{
    private SpriteRenderer Sr { get; set; }
    [SerializeField] protected GameObject groundDetectObject;
    [SerializeField] protected GameObject wallDetectObject;
    public float GroundDetectDist { get; private set; }
    protected float WallDetectDist { get; set; }
    private Vector2 currentDir;
    public virtual Vector2 CurrentDir
    {
        get => currentDir;
        protected set => currentDir = value;
    }
    

    protected virtual void Awake()
    {
        Sr = GetComponentInChildren<SpriteRenderer>();
        GroundDetectDist = 0.3f;
        WallDetectDist = 0.3f;
    }
    
    
    
    public void Flip(bool cond)
    {
        if (cond)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else //원래 상태
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
    
    public bool IsGroundDetect()
    {
        if (Physics2D.Raycast(groundDetectObject.transform.position, Vector2.down, GroundDetectDist, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
    
    public virtual bool IsWallDetect()
    {
        if (Physics2D.Raycast(wallDetectObject.transform.position, new Vector2(CurrentDir.x, 0f), GroundDetectDist, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
    
    public bool IsPrepareJumpEnd()
    {
        if (Physics2D.Raycast(groundDetectObject.transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
    

    protected virtual void TransitionMoveState()
    {
    }

    protected virtual bool CheckStateCanMove()
    {
        return false;
    }
}
