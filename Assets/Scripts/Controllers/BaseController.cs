using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected StatManager StatManager { get; set; }
    private SpriteRenderer Sr { get; set; }
    [SerializeField] protected GameObject groundDetectObject;
    [SerializeField] public GameObject wallDetectObject;
    public float GroundDetectDist { get; private set; }
    protected float WallDetectDist { get; set; }
    private Vector2 currentDir = Vector2.right;
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
        StatManager = GetComponent<StatManager>();
    }

    public enum EDir
    {
        Left,
        Right
    }
    public void Flip(EDir cond)
    {
        if (cond == EDir.Left)
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
