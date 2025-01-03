using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    public override void Enter()
    {
       enemy.animator.Play("Idle");
    }

    public override State CheckTransitions()
    {

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
