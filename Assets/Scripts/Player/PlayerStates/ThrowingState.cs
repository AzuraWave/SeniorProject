using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingState : AttackBaseState
{
    public override bool IsActionState => true;
    private bool shurikenThrown = false;
    public override void Enter()
    {
        attackData = new AttackData(true, false, 20, 0, 2, 1);
        shurikenThrown = false;
        timeSinceAttackStarted = 0f;
        string animationName = $"Throwing";
        PlayAnimation(animationName); 
        StartAttack();
        SFXManager.Instance.PlaySound2D("Throwing", false);
        
    }

    public override void Execute()
    {
        if (shurikenThrown == false){
            player.objectSpawner.SpawnShuriken(player.transform.localScale.x > 0 ? Vector2.right : Vector2.left);
            shurikenThrown = true;
        }
        if(IsAttackComplete()){
            EndAttack();
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
    }
}
