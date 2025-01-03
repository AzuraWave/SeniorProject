using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AirState : State
{
public override void Enter()
{
        if (player.jumpPressed)
        {
            player.body.velocity = new Vector2(player.body.velocity.x, player.stats.jumpForce);
            player.jumpPressed = false; // Reset the jump flag to prevent repeated jumps.
        }
}

    public override void Execute()
    {
    // Check if at the jump peak (vertical velocity close to zero)
    if (transitionFrame())
    {
        PlayAnimation("in-air-transition");
    }
    else if (player.body.velocity.y < 0)
    {
        PlayAnimation("in-air-descending");
    }
    else
    {
        PlayAnimation("in-air-ascending");
    }

    
 
    }
    public override State CheckTransitions()
    {

        if (player.grounded)
        {
            if (player.IsMoving)
            {
                return stateMachine.GetState<WalkState>();
            }
        return stateMachine.GetState<IdleState>();
        } else {
            if (Input.GetButtonDown("Fire1"))
            {
            return player.stateMachine.GetState<AirAttackState>();
        }
        }
        return null;
    }

    public override void Exit()
    {
    }

    public bool transitionFrame(){
        return Mathf.Abs(player.body.velocity.y) < 3f;
    }

    protected void PlayAnimation(string animationName)
    {
        if (!player.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            player.animator.Play(animationName);
        }
    }


}
