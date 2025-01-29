using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackState : AttackBaseState
{
    public override bool IsActionState => true;
    
     public override void Enter()
    {
        attackData = new AttackData(true, false, 20, 0, 2, 1);
        PlayAnimation("AirAttack");
        unBufferAttack();
        StartAttack();
        SFXManager.Instance.PlaySound2D("SwordSlash");
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            BufferAttack(); 
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

