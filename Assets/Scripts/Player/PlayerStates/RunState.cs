using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{

    public override void Enter()
    {
        player.animator.Play("Run");
        player.isRunning = true;
        SFXManager.Instance.PlaySound2D("Running", true);
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

    public override void Exit()
    {
        player.isRunning = false;
        SFXManager.Instance.StopSound2D();
    }
}
