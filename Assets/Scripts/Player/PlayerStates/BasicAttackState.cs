using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackState : AttackBaseState
{
    private int attackIndex = 0;
    public override bool IsActionState => true;
    public override void Enter()
    {
        attackData = new AttackData(true, false, 30, 0, 2, 1);

        timeSinceAttackStarted = 0f;
        string animationName = $"Attack{attackIndex + 1}";
        PlayAnimation(animationName);
        attackIndex = (attackIndex + 1) % 3; 
        unBufferAttack();   
        StartAttack();
        SFXManager.Instance.PlaySound2D("SwordSlash");
        
    }

    public override void Execute()
    {
        timeSinceAttackStarted += Time.deltaTime;
        if (Input.GetMouseButton(0) && timeSinceAttackStarted > 0.2)
        {
            BufferAttack(); 
        }
        if (Input.GetMouseButtonDown(1))
        {
            blockBuffered = true;
        }
        if (IsAttackComplete())
        {
            if (HasBufferedAttack())
            {
                Enter(); 
            }
            else
            {
                EndAttack();
            }
        }
    }

    public override State CheckTransitions()
    {
        if (!IsAttacking){

            return player.grounded ? player.stateMachine.GetState<IdleState>() : player.stateMachine.GetState<AirState>();
        }
        if (blockBuffered && timeSinceAttackStarted > 0.2f && timeSinceAttackStarted < 0.5f)
        {
            blockBuffered = false; 
            return player.stateMachine.GetState<BlockingState>();
        }
        return null;
    }

    public override void DetectAndDamageEnemies(){
        
        float facingDirection = player.transform.localScale.x > 0 ? 1 : -1;

        
        Vector2 attackAreaOrigin = (Vector2)player.transform.position + (Vector2)player.transform.right * facingDirection * attackData.range / 2;

        
        Vector2 attackAreaSize = new Vector2(attackData.range, attackData.height); 

        
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackAreaOrigin, attackAreaSize, 0, player.enemyLayer);

        
        foreach (var enemyCollider in hitEnemies)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("Enemy hit: " + enemyCollider.name);
                damageable.OnAttackReceived(attackData); 
            }
        }
    }

    public override void Exit()
    {
        SFXManager.Instance.StopSound2D();
    }
}

