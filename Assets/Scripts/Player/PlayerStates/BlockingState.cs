using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingState : State
{
    public override bool IsActionState => true;
    public override void Enter()
    {
        player.canMove = false;
        player.blockManager.StartParryWindow();
        player.animator.Play("Block"); // Play block animation
        player.blockManager.isBlocking = true; // Set blocking state
        player.posturePause = true;
        
        
    }

    public override void Execute()
    {
        if(player.grounded)
        {
            player.body.velocity = Vector2.zero;
        }
        
    }

    public override State CheckTransitions()
    {
        if (Input.GetMouseButtonUp(1) || player.stats.Posture <= 0)
        {
            return stateMachine.GetState<IdleState>();
        }
        return null;
    }


    public override void Exit()
    {
        base.Exit();
        player.canMove = true;
        player.blockManager.ResetParryWindow(); // Reset block state
        player.blockManager.isBlocking = false; 
        player.posturePause = false;
    }
}
