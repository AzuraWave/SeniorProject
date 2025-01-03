using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{

    public override void Enter()
    {
        player.animator.Play("Walking");
    }
    public override void Execute()
    {
        
    }

    public override State CheckTransitions()
    {

        if (Input.GetButtonDown("Fire1") )
        {
        return player.stateMachine.GetState<BasicAttackState>();
        }
        if ( player.grounded)
        {   if (!player.IsMoving)
            {
                return stateMachine.GetState<IdleState>();
            } else if (player.IsMoving ){
                 return stateMachine.GetState<WalkState>();
            }
           
        }else 
        {
            return stateMachine.GetState<AirState>();
        }
        return null;
    }

    public override void FixedExecute()
    {
 
    }
}
