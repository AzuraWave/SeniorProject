using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowingState : State
{
    private bool IsCooldown;
    private bool IsAttacking;

    private float cooldownTime = 0.8f; 
    private float cooldownTimer;
       private bool hasAnimationCompleted = false;
    public void StartAttack()
    {
        enemy.animator.Play("Throw");
        IsAttacking = true;
        IsCooldown = false;
        enemy.enemyObjectSpawner.SpawnShuriken(enemy.transform.localScale.x > 0 ? Vector2.right : Vector2.left);
        SFXManager.Instance.PlaySound3D("Throwing", enemy.transform.position);
    }

    public void EndAttack()
    {
        IsAttacking = false;
        StartCooldown();
    }

    private void StartCooldown()
    {
        IsCooldown = true;
        cooldownTimer = cooldownTime;
    }
    public override void Enter()
    {
        
        hasAnimationCompleted = false;
        enemy.transform.localScale = new Vector3(enemy.transform.position.x < enemy.getPlayer().transform.position.x ? 1 : -1, enemy.transform.localScale.y, enemy.transform.localScale.z);
        StartAttack();
        
    }

    public override void Execute()
    {
        if (IsAttacking && IsAttackComplete())
        {
            EndAttack();
        }

        if (IsCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                IsCooldown = false;
            }
        }
    }

    public override State CheckTransitions()
    {
        if (!IsAttacking && !IsCooldown)
        {
            if (enemy.PlayerInDetectionRange){
                return enemy.stateMachine.GetState<EnemyChaseState>();
            } else {
                return enemy.stateMachine.GetState<EnemyPatrolState>();
            }
        }
        

        return null;
    }
    
 

protected bool IsAttackComplete()
{
    AnimatorStateInfo stateInfo = enemy.animator.GetCurrentAnimatorStateInfo(0);

    
    if (stateInfo.normalizedTime >= 1.0f && !hasAnimationCompleted)
    {
        hasAnimationCompleted = true; 
        return true;
    }

    return false;
}


    public override void Exit()
    {
        IsAttacking = false;
        IsCooldown = false;
    }
}
