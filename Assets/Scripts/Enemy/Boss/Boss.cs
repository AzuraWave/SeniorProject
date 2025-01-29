using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    public Transform player;
    public bool isFlipped = false;
    public Animator animator;
    public int Health = 500;
    public float Posture = 200;
    public bool hurtcooldown = false;
    public bool inFlamed = false;
    public bool invulnerable = false;
public bool canAttack;
public bool didDie;
    private void Start() {
        animator = GetComponent<Animator>();
        canAttack = true;
        Health = 500;
    }
    private void Update() {
        if(Posture <= 0)
        {
            animator.SetTrigger("Stagger");
            hurtcooldown = true;
            Posture = 200;
        }

        if (Health <= 0 && !didDie)
        {
            animator.SetBool("Dead", true);
            animator.SetTrigger("Dead");
            didDie = true;
            Destroy(gameObject, 5f);
        }

        if (Health <= 250 && !inFlamed){

            animator.SetTrigger("Phase2");
            animator.SetBool("Flame", true);
        }
    }
    public void lookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        
        if (transform.position.x < player.position.x && isFlipped){
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }else if (transform.position.x > player.position.x && !isFlipped){
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void OnAttackReceived(AttackData attackData)
    {
        if (!invulnerable){

            Health -= attackData.healthDamage;
         ReducePosture(attackData.postureDamage);
            if (hurtcooldown == false && animator.GetBool("isAttacking") == false)
            {
            animator.SetTrigger("Hurt");
            hurtcooldown = true;
            
            
            }
            Debug.Log("Boss Health: " + Health);
            SFXManager.Instance.PlaySound3D("FleshHit", transform.position);
            
        }
    }

    public void startHurtCooldown()
    {
        StartCoroutine(HurtCooldown());
    }

    public IEnumerator HurtCooldown()
    {
        
        yield return new WaitForSeconds(0.5f);
        hurtcooldown = false;
    }

    public void AttackCooldownCoroutine(float attackCooldown)
    {
        StartCoroutine(AttackCooldown(attackCooldown));
    }
    System.Collections.IEnumerator AttackCooldown(float attackCooldown)
    {
        // Disable attack
        canAttack = false;

        // Wait for the cooldown period
        yield return new WaitForSeconds(attackCooldown);

        // Re-enable attack
        canAttack = true;
    }

    public void ReducePosture(int damage)
    {
        Posture = Mathf.Clamp(Posture - damage, 0, 200);
    }
}
