using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 20; 
    public LayerMask enemyLayer; // Assign this in the Inspector
    private Vector2 direction;
    private AttackData attackData = new AttackData(true,true, 40, 0, 2, 1); 

    public void Initialize(Vector2 direction)
    {
        this.direction = direction.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            
            IDamageable enemy = collision.GetComponent<IDamageable>();
            if (enemy != null)
            {
                AttackData temp = attackData;
                if (enemy.GetType() == typeof(Boss))
                {
                    Boss boss = collision.GetComponent<Boss>();
                    
                    
                        temp = new AttackData(true, true, 10, 0, 2, 1);
                    
                }
                enemy.OnAttackReceived(temp); 
            }

            Destroy(gameObject); 
        }
    }
}
