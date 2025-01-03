using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private bool isParryEnabled = false;
    private bool isParryWindowActive = false; // Flag to track the parry window status
    private float parryWindow;
    private float parryTimer = 0f; // Timer to track parry window duration

    public bool isBlocking = false;
    public PlayerController player;


    private void Start (){
        parryWindow = player.stats.parryWindow;
    }

    private void Update()
    {
        // Check if parry window is active and decrement timer
        if (isParryWindowActive)
        {
            parryTimer -= Time.deltaTime;
            if (parryTimer <= 0f)
            {
                ResetParryWindow();
            }
        }

            AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("BlockedHit") && stateInfo.normalizedTime >= 1.0f) // Animation has finished
            {
                player.animator.Play("Block");
                
            }
        
    }

    public void StartParryWindow()
    {
        isParryEnabled = true;
        isParryWindowActive = true; // Mark window as active
        parryTimer = parryWindow;   // Set timer for the parry duration

    }

    public void ResetParryWindow()
    {
        isParryEnabled = false;
        isParryWindowActive = false; // Mark window as inactive
        parryTimer = 0f;             // Reset timer
    }


    public bool IsParryWindowActive()
    {
        return isParryWindowActive;
    }

    public void HandleAttack(bool canParry, bool canBlock, int healthDamage, int postureDamage)
    {
        if (isParryEnabled && canParry)
        {
            Debug.Log("Parried");
            player.animator.Play("BlockedHit");
        }
        else if (isBlocking && canBlock)
        {
            Debug.Log("Blocked");
            player.animator.Play("BlockedHit");
            player.stats.TakePostureDamage(postureDamage);
        }
        else
        {
            player.stats.TakeHealthDamage(healthDamage);
            player.stats.TakePostureDamage(postureDamage);

            if (player.stats.health > 0)
            {
                player.pendingHurtStateTransition = true;
            }
        }  
    }
}
