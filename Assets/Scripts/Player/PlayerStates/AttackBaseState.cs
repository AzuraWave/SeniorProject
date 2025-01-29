using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBaseState : State
{
    protected float timeSinceAttackStarted = 0f;
    private bool isAttackBuffered = false; // Buffer for the next attack

    public bool IsAttacking { get; private set; }
    public bool blockBuffered = false;

    public AttackData attackData = new AttackData();
    

    public void StartAttack()
    {
        IsAttacking = true;
    }

    public void EndAttack()
    {
        IsAttacking = false;
    }
    protected void BufferAttack()
    {
        isAttackBuffered = true;
    }
    protected void unBufferAttack()
    {
        isAttackBuffered = false;
    }

    protected bool HasBufferedAttack()
    {
        return isAttackBuffered;
    }

    protected void PlayAnimation(string animationName)
    {
        if (!player.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            player.animator.Play(animationName);
        }
    }

    protected bool IsAttackComplete()
    {
        
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        bool result = stateInfo.normalizedTime >= 1.0f;

    return result;
    }

    public abstract void DetectAndDamageEnemies();


}

