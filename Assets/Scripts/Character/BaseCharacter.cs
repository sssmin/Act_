using System.Collections;
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
    private SpriteRenderer Sr { get; set; }
    public Animator Animator { get; set; }
    public StateMachine StateMachine { get; set; }
    public StatManager StatManager { get; protected set; }
    protected Coroutine HitEffectCoroutine { get; set; }

    public float MoveSpeed => StatManager.characterStats.moveSpeed.Value;
    

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<Animator>();
        StateMachine = GetComponent<StateMachine>();
        InstId = GetInstanceID();
        StatManager = GetComponent<StatManager>();
    }

    protected virtual void Start()
    {
        InitState();
        StatManager.Character = this;
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

    public void HitEffect()
    {
        if (HitEffectCoroutine != null)
        {
            StopCoroutine(HitEffectCoroutine);
            Sr.color = Color.white;
        }
        HitEffectCoroutine = StartCoroutine(CoActivateHitEffect());
    }
    
    public IEnumerator CoActivateHitEffect()
    {
        Sr.color = Color.red;
        while (true)
        {
            yield return null;
            Sr.color = Color.Lerp(Sr.color, Color.white, Time.deltaTime);
            if (Sr.color == Color.white)
                yield break;
        }
    }
    
    
    
}
