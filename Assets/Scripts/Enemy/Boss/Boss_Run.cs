using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{

    public float speed = 2.5f;
    public float attackRange = 1.5f;
    Transform player;
    Boss boss;
    Boss_weapon weapon;
    Rigidbody2D rb;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
         boss = animator.GetComponent<Boss>();
        weapon = animator.GetComponent<Boss_weapon>();
        if (weapon.SpecialAttack)
        {
           animator.SetInteger("AttackType", 3);
        } else {
             animator.SetInteger("AttackType", UnityEngine.Random.Range(0, 3));
        }
         
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.lookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        if ( Vector2.Distance(rb.position, player.position) > attackRange){
            rb.MovePosition(newPos);
        }

        if (Vector2.Distance(rb.position, player.position) <= attackRange && boss.canAttack)
        {   
            
            animator.SetTrigger("Attack");
            

        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Attack");
       
    }

   
}
