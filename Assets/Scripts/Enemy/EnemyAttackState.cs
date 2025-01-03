using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State
{
    private int attackIndex = 0;

    private bool IsCooldown;
    private bool IsAttacking;

    private float cooldownTime = 0.5f; // Duration of the cooldown in seconds
    private float cooldownTimer;

    public void StartAttack()
    {
        IsAttacking = true;
        IsCooldown = false;
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
        string animationName = $"Attack{attackIndex + 1}";
        enemy.PlayAnimation(animationName);
        attackIndex = (attackIndex + 1) % 3;

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
            return enemy.stateMachine.GetState<EnemyChaseState>();
        }

        return null;
    }
    
    protected bool IsAttackComplete()
    {
        
        AnimatorStateInfo stateInfo = enemy.animator.GetCurrentAnimatorStateInfo(0);
        bool result = stateInfo.normalizedTime >= 1.0f;

    return result;
    }

    public override void Exit()
    {
        IsAttacking = false;
        IsCooldown = false;
    }
    
}
