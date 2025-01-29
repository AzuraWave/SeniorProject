using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    public Boss boss;
    public AttackData attackData = new AttackData(true, true, 20, 25, 5f, 1f, 2f);
    public bool attackParried;

    public void Start()
    {
        boss = GetComponentInParent<Boss>();
        if (boss.inFlamed){

        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.GetComponent<PlayerController>();
            float direction =  transform.position.x-player.transform.position.x ;
            attackData.knockbackDirection.x = direction;
            attackData.knockbackDirection.y = 0;
            
            player.OnAttackReceived(attackData);
            if (player.blockManager.GetIsParryEnabled() && attackData.canParry == true)
            {
                attackParried = true;
            }
        }
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        if(attackParried == true){
            boss.ReducePosture(attackData.postureDamage);
            attackParried = false;
        }
    }
    
   
}
