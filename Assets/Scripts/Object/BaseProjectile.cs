using UnityEngine;

public class BaseProjectile : BaseActor
{
    protected float AttackCoefficient { get; set; }
   
    
    public virtual void Init(Vector2 dir, BaseController inOwner, float inAttackCoefficient)
    {
        OwnerController = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        AttackCoefficient = inAttackCoefficient;
        if (SpriteRenderer)
        {
            if (dir.x < 0f)
                SpriteRenderer.flipX = true;
            else
                SpriteRenderer.flipX = false;
        }
    }
}
