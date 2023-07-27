using UnityEngine;

public class BaseActor : MonoBehaviour
{
    protected Rigidbody2D Rb { get; set; }
    public float speed;
    [HideInInspector] public BaseController OwnerController;
    protected SpriteRenderer SpriteRenderer { get; set; }
 
    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    
}
