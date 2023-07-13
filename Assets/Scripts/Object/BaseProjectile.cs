using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : BaseActor
{
    
    public float AttackCoefficient { get; set; }
    
    
    
    public virtual void Init(Vector2 dir, BaseController inOwner, float inAttackCoefficient)
    {
        OwnerController = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        AttackCoefficient = inAttackCoefficient;
    }
}
