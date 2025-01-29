using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public PlayerController player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.grounded  && player.currentState.IsActionState == false)
        {
            player.stateMachine.TransitionTo(player.stateMachine.GetState<ThrowingState>());
        }

        if(player.canDash && Input.GetKeyDown(KeyCode.LeftShift)){
            player.stateMachine.TransitionTo(player.stateMachine.GetState<DashState>());
        }
    }
    
}
