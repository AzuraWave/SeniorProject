using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackState : AttackBaseState
{
    public override bool IsActionState => true;
     public override void Enter()
    {
        PlayAnimation("AirAttack");
        unBufferAttack();
        StartAttack();
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.X)) // Replace KeyCode.X with your attack button
        {
            BufferAttack(); // Buffer the next attack
        }

        if (IsAttackComplete())
        {
            if (HasBufferedAttack())
            {
                Enter(); // Continue the attack with buffered input
            }
            else
            {
                EndAttack();
            }
        }
    }

    public override State CheckTransitions()
    {
        if (!IsAttacking){

            return player.grounded ? player.stateMachine.GetState<IdleState>() : player.stateMachine.GetState<AirState>();
        }
        return null;
    
    }


}

