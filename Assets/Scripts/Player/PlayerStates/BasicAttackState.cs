using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackState : AttackBaseState
{
    private int attackIndex = 0;
    public override bool IsActionState => true;

    public override void Enter()
    {
        timeSinceAttackStarted = 0f;
        string animationName = $"Attack{attackIndex + 1}";
        PlayAnimation(animationName);
        attackIndex = (attackIndex + 1) % 3; // Cycle through attacks
        unBufferAttack();   
        StartAttack();
        
    }

    public override void Execute()
    {
        timeSinceAttackStarted += Time.deltaTime;
        if (Input.GetMouseButton(0) && timeSinceAttackStarted > 0.2) // Replace KeyCode.X with your attack button
        {
            BufferAttack(); // Buffer the next attack
        }
        if (IsAttackComplete())
        {
            if (HasBufferedAttack())
            {
                Enter(); // Restart attack cycle with buffered input
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

