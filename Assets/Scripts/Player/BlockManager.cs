using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private bool isParryEnabled = false;
    private bool isParryWindowActive = false; 
    private float parryWindow;
    private float parryTimer = 0f; 
    public bool isBlocking = false;
    
    public PlayerController player;

    public bool GetIsParryEnabled()
    {
        return isParryEnabled;
    }   
    private void Start (){
        parryWindow = player.stats.parryWindow;
    }

    private void Update()
    {
        
        if (isParryWindowActive)
        {
            parryTimer -= Time.deltaTime;
            if (parryTimer <= 0f)
            {
                ResetParryWindow();
            }
        }

            AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("BlockedHit") && stateInfo.normalizedTime >= 1.0f) 
            {
                player.animator.Play("Block");
                
            }
        
    }

    public void StartParryWindow()
    {
        isParryEnabled = true;
        isParryWindowActive = true; 
        parryTimer = parryWindow;   

    }

    public void ResetParryWindow()
    {
        isParryEnabled = false;
        isParryWindowActive = false; 
        parryTimer = 0f;             
    }


    public bool IsParryWindowActive()
    {
        return isParryWindowActive;
    }


    public void HandleAttack(AttackData attackData)
    {
        if (isParryEnabled && attackData.canParry)
        {
            Debug.Log("Parried");
            SFXManager.Instance.PlaySound3D("Parrying", player.transform.position);
            player.animator.Play("BlockedHit");
        }
        else if (isBlocking && attackData.canBlock)
        {
            Debug.Log("Blocked");
            player.animator.Play("BlockedHit");
            SFXManager.Instance.PlaySound3D("Blocking", player.transform.position);
            player.stats.TakePostureDamage(attackData.postureDamage);
        }
        else
        {
            SFXManager.Instance.PlaySound3D("FleshHit", transform.position);
            player.stats.TakeHealthDamage(attackData.healthDamage);
            player.stats.TakePostureDamage(attackData.postureDamage);
            if(attackData.knockbackPower > 0)
            {
                player.ApplyKnockback(attackData.knockbackDirection, attackData.knockbackPower);
            }
            if (player.stats.health > 0)
            {
                player.pendingHurtStateTransition = true;
            }
            if (player.stats.Posture < 0){
                player.PausePostureCoroutine();
            }
        }  
    }
    
}
