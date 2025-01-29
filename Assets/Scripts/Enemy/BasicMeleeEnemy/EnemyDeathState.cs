using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState: State
{

    
    public override void Enter()
    {
        enemy.PlayAnimation("Death");  

            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;  
            rb.isKinematic = true;      
        }

        
        Collider2D collider = enemy.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; 
        }

        
        
    }
    public override void Execute()
    {
    }

    public override void FixedExecute()
    {

    }

    public override State CheckTransitions()
    {
        return null;
    }
}
