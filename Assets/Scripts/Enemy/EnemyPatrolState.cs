using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State
{
    [Header("Enemy")]
    [Header("Movement Parameters")]
    private Vector3 initScale;
    private Transform targetNode;
    private float switchDelay = 2f;  // Time to wait before switching nodes
    private float timer = 0f;

    private bool Idling;
    public override void Enter() 
    {
        initScale = enemy.transform.localScale;
        enemy.PlayAnimation("Run");
        targetNode = enemy.LeftNode;  // Start patrolling towards the LeftNode
        Idling = false;
    }

    public override void Execute() 
    {
        if (targetNode == null)
        {
            Debug.LogWarning("TargetNode is null. Please assign LeftNode and RightNode.");
            return;
        }

        if(!Idling)
        {
            // Move towards the target node
            MoveInDirection(enemy.transform.position.x < targetNode.position.x ? 1 : -1);
        }

        // Check if the enemy has reached the target node
        if (Mathf.Abs(enemy.transform.position.x - targetNode.position.x) < 0.1f)
        {
            enemy.PlayAnimation("Idle");
            Idling = true;
            if (timer >= switchDelay)
            {
                SwitchToNextNode();
                timer = 0f;  // Reset the timer after switching nodes
                Idling = false;
                enemy.PlayAnimation("Run");
            }
            else
            {
                // Increment the timer
                timer += Time.deltaTime;
            }
        }
    }

    public override State CheckTransitions()
    {
        if (enemy.PlayerInDetectionRange)
        {
            return enemy.stateMachine.GetState<EnemyChaseState>();
        }
        return null;
    }

    public override void FixedExecute() {}

    public override void Exit() {}

    private void MoveInDirection(int direction)
    {
        enemy.transform.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        enemy.transform.position += new Vector3(direction, 0, 0) * Time.deltaTime * enemy.stats.speed;
    }

    private void SwitchToNextNode()
    {
        // Switch between LeftNode and RightNode
        targetNode = targetNode == enemy.LeftNode ? enemy.RightNode : enemy.LeftNode;
    }
}
