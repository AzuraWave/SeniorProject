using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class IdleState : State
{

    public override void Enter()
    {
       player.animator.Play("Idle");
    }

    public override State CheckTransitions()
    {
        if (Input.GetButtonDown("Fire1"))
        {
        return player.stateMachine.GetState<BasicAttackState>();
        }
        if (player.IsMoving) return stateMachine.GetState<WalkState>();
        if (!player.grounded) return stateMachine.GetState<AirState>();
        return null;
    }

    public override void Execute()
    {
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {
    }
}
