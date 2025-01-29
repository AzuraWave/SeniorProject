using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WalkState : State
{
    private float timer;
    public override void Enter()
    {
        player.animator.Play("Walking");
        timer = 0f;
        SFXManager.Instance.PlaySound2D("Walking", true);
    }
    public override void Execute()
    {
        timer += Time.deltaTime;
        
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
            } else if (timer >= 0.5f ){
                 return stateMachine.GetState<RunState>();
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
        SFXManager.Instance.StopSound2D();
    }
}
