using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{

    public override void Enter()
    {
        player.animator.Play("Death");
        
    }
    public override void Execute()
    {
    }

    public override void FixedExecute()
    {

    }

    public override State CheckTransitions()
    {
        return null;
    }

}
