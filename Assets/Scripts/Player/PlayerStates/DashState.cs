using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State
{
    public override void Enter()
    {
        player.invulnerable = true;
        player.animator.Play("Dash");
        player.StartCoroutine(Dash());
        SFXManager.Instance.PlaySound2D("Dashing");
    }

    public override State CheckTransitions()
    {
        if(player.IsCurrentAnimationComplete("Dash")){
            return player.stateMachine.GetState<IdleState>();
        }
        return null;
    }

    public override void Execute()
    {
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {

        SFXManager.Instance.StopSound2D();
        player.invulnerable = false;
    }
    public IEnumerator Dash(){
        player.canDash = false;
        player.isDashing = true;
        float originalGravity = player.body.gravityScale;
        player.body.gravityScale = 0f;
        player.body.velocity = new Vector2(player.transform.localScale.x * player.dashingPower, 0f);
        yield return new WaitForSeconds(player.dashDuration);
        player.body.gravityScale = originalGravity;
        player.isDashing = false;
        player.body.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(player.dashCooldown);
        player.canDash = true;

    }
}
