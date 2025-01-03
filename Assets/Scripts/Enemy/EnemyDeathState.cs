using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState: State
{
    public override void Enter()
    {
        enemy.PlayAnimation("Death");
        
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
