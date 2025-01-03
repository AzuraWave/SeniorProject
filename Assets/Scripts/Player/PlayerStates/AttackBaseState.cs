using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBaseState : State
{
    private float attackRange = 2;
 // The total duration of the attack animation
    protected float timeSinceAttackStarted = 0f;
    private bool isAttackBuffered = false; // Buffer for the next attack

    public bool IsAttacking { get; private set; }
    

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

    public void DetectAndDamageEnemies()
    {
    // Determine the facing direction (-1 for left, 1 for right)
    float facingDirection = player.transform.localScale.x > 0 ? 1 : -1;

    // Define the attack area origin based on the facing direction
    Vector2 attackAreaOrigin = (Vector2)player.transform.position + (Vector2)player.transform.right * facingDirection * attackRange / 2;

    // Define the attack area size
    Vector2 attackAreaSize = new Vector2(attackRange, 1f); // Adjust the height (1f) as needed

    // Perform a BoxCast or OverlapBox to detect enemies in front of the player
    Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackAreaOrigin, attackAreaSize, 0, player.enemyLayer);

    // Loop through all detected enemies and apply damage
    foreach (var enemyCollider in hitEnemies)
    {
        Enemy enemy = enemyCollider.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            enemy.OnAttackReceived(true, true, 10, 10); // Apply damage, change the values as needed
        }
    }
}


}

