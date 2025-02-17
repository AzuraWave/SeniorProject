using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Transition : StateMachineBehaviour
{
    Transform player;
    Boss boss;
    Boss_weapon weapon;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
         boss = animator.GetComponent<Boss>();
        weapon = animator.GetComponent<Boss_weapon>();
        boss.invulnerable = true;
         SFXManager.Instance.PlaySound3D("BossShout", animator.transform.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       boss.invulnerable = false;
    }
}
