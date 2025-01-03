using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State
{
    public override void Enter()

    {
        if (enemy.animator == null) Debug.LogError("Animator is not assigned.");
        if (enemy.getPlayer() == null) Debug.LogError("Player reference is not assigned.");
        if (enemy.stats == null) Debug.LogError("Stats reference is not assigned.");
            enemy.animator.Play("Run");
    }
    public override void Execute()
    {
        
        
    }

    public override void FixedExecute()
    {
        if (enemy.getPlayer() != null) 
        {
            MoveInDirection(enemy.transform.position.x < enemy.getPlayer().transform.position.x ? 1 : -1);
        }
        else
        {
            Debug.LogError("Player reference is missing on enemy.");
        }
    }

    public override State CheckTransitions()
    {

        if (enemy.PlayerInAttackRange)
        {
        return enemy.stateMachine.GetState<EnemyAttackState>();
        }
        
        if (!enemy.PlayerInDetectionRange)
        {
            return enemy.stateMachine.GetState<EnemyPatrolState>();
        }
        
        return null;
    }

    private void MoveInDirection(int direction)
    {
        enemy.transform.position += new Vector3(direction * enemy.stats.speed * Time.deltaTime, 0, 0);
        enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x) * direction, enemy.transform.localScale.y, enemy.transform.localScale.z);
        
    }
}
