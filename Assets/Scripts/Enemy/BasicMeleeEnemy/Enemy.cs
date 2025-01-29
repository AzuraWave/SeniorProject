using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public LayerMask playerLayer;
    public BoxCollider2D enemyCollider;
    public EnemyObjectSpawner enemyObjectSpawner;
    public Rigidbody2D rb;
    [SerializeField] private float attackRange;
    [SerializeField] private float colliderDistance;

    private float health;

    private bool isAlive ;
    public Animator animator;
    public StateMachine stateMachine;
    [SerializeField]private PlayerController player;
    public bool IsEnemyMoving => Mathf.Abs(rb.velocity.x) > 0;
    public bool PlayerInDetectionRange;
    public bool PlayerInThrownRange;
    public bool PlayerInAttackRange;

    private bool pendingHurtStateTransition;

    public State currentState => stateMachine?.CurrentState;
    public string currentStateName;

    [Header ("Enemy Stats")]
    public Enemy1Stats stats;
    private AttackData basicAttack = new AttackData(true, true, 25, 20, 2,2);
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    public bool hasThrown;

    [Header ("Patrol Points")]
    public Transform LeftNode;
    public Transform RightNode;

    private bool healthRecovered = false;
    void Start()
    {
        isAlive = true;
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
        var states = new List<State>
        {
            new EnemyIdleState(),
            new EnemyChaseState(),
            new EnemyAttackState(),
            new EnemyPatrolState(),
            new EnemyDeathState(),
            new EnemyHurtState(),
            new EnemyThrowingState()
        };
        stateMachine.AddGlobalTransition(() => {
            if (health <= 0){
                isAlive = false;
                return stateMachine.GetState<EnemyDeathState>();
            }
            return null;
        });
        stateMachine.AddGlobalTransition(() =>
        {
            if (pendingHurtStateTransition)
            {
                pendingHurtStateTransition = false; 
                return stateMachine.GetState<EnemyHurtState>();
            }
            return null;
        });


        
        foreach (var state in states)
        {
            state.Setup(stateMachine, this);
            stateMachine.AddState(state);
        }

            
            stateMachine.Initialize(stateMachine.GetState<EnemyPatrolState>());


        health = stats.healthMAX;
        stats.setPosture();
    }
        

    public void ApplyKnockback(Vector2 direction)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(KnockbackRecovery());
        }
    }

    private IEnumerator KnockbackRecovery()
    {
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero; 
        isKnockedBack = false;
    }

    void Update()
    {

        stateMachine.Execute();
        PlayerInAttackRange =  CheckPlayerInAttackRange();
        CheckPlayerInDetectionRange();
        MovementDirection();
        currentStateName = currentState?.GetType().Name ?? "None";
        if (isAlive == false && healthRecovered == false){
            player.stats.increaseHealth(40);
            healthRecovered = true;
        }
    }

    void FixedUpdate()
    {
        stateMachine.FixedExecute();
    }

    private void CheckPlayerInDetectionRange(){
        if (player == null)
        {
        return; 
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer < stats.DetectionRange)
        {
            PlayerInDetectionRange = true;
        }
        else
        {
            PlayerInDetectionRange = false;
        }

        if ( distanceToPlayer > stats.DetectionRange/2 && distanceToPlayer < stats.DetectionRange){
            PlayerInThrownRange = true;
        }
        else
        {
            PlayerInThrownRange = false;
        }
    }

    private bool CheckPlayerInAttackRange()
{
    Vector2 boxCastOrigin = (Vector2)enemyCollider.bounds.center + new Vector2(transform.localScale.x * colliderDistance, 0);
    Vector2 boxCastSize = new Vector2(enemyCollider.bounds.size.x * attackRange, enemyCollider.bounds.size.y);

   
    RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0, Vector2.zero, 0, playerLayer);


    return hit.collider != null;
}



    private void OnDrawGizmos() {
        if (enemyCollider != null)
    {
        Gizmos.color = Color.red;
        Vector3 cubeCenter = enemyCollider.bounds.center 
                             + transform.right * transform.localScale.x * colliderDistance;
        Vector3 cubeSize = new Vector3(attackRange, enemyCollider.bounds.size.y, enemyCollider.bounds.size.z);
        Gizmos.DrawWireCube(cubeCenter, cubeSize);
    }

    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, stats.DetectionRange);
    }
    private void MovementDirection()
    {
        if (rb.velocity.x != 0)
        {
        float direction = Mathf.Sign(rb.velocity.x);
        transform.localScale = new Vector3(direction, 1, 1);
        }
    }
    private void DamagePlayer()
    {
        if (PlayerInAttackRange){
            player.OnAttackReceived(basicAttack);
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            animator.Play(animationName);
        }
    }

    public bool IsAnimationComplete()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    public PlayerController getPlayer(){
        return player;
    }

    public void OnAttackReceived(AttackData attackData)
    {
        SFXManager.Instance.PlaySound3D("FleshHit", transform.position);
        health -= attackData.healthDamage;
        stats.TakePostureDamage(attackData.postureDamage);
        ApplyKnockback(transform.position - player.transform.position);
        if (health > 0) 
        {
            if (currentStateName != "EnemyHurtState") 
            {
            pendingHurtStateTransition = true;
            }
        }
        
    }
}


