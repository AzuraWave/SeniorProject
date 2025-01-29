using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : State
{

    public override void Enter()
    {
        enemy.PlayAnimation("Hurt");
    }

    public override void Execute()
    {
    }

    public override State CheckTransitions()
    {
        if (enemy.IsAnimationComplete())
        {
           return enemy.stateMachine.GetState<EnemyChaseState>();
        }

        return null;
    }

    public override void Exit()
    {
    }
}
