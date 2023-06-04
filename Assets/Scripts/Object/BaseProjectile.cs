using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : BaseActor
{
    private int damage;

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public virtual void Init(Vector2 dir, BaseController inOwner, int inDamage)
    {
        OwnerController = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        Damage = inDamage;
    }
}
