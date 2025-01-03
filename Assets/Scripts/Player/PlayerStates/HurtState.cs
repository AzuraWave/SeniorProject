using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    public override void Enter()
    {
        player.animator.Play("Hurt");
        
    }
    public override void Execute()
    {
    }

    public override void FixedExecute()
    {

    }

    public override State CheckTransitions()
    {
        if (player.IsCurrentAnimationComplete("Hurt"))
        {
            return player.stateMachine.GetState<IdleState>();
        }
        return null;
    }
}
