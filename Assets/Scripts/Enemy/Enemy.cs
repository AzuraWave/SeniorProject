using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LayerMask playerLayer;
    public BoxCollider2D enemyCollider;

    public Rigidbody2D rb;
    [SerializeField] private float attackRange;
    [SerializeField] private float colliderDistance;

    public Animator animator;
    public StateMachine stateMachine;
    [SerializeField]private PlayerController player;
    public bool IsEnemyMoving => Mathf.Abs(rb.velocity.x) > 0;
    public bool PlayerInDetectionRange;
    public bool PlayerInAttackRange;

    private bool pendingHurtStateTransition;

    public State currentState => stateMachine?.CurrentState;
    public string currentStateName;

    [Header ("Enemy Stats")]
    public Enemy1Stats stats;

    [Header ("Patrol Points")]
    public Transform LeftNode;
    public Transform RightNode;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
        var states = new List<State>
        {
            new EnemyIdleState(),
            new EnemyChaseState(),
            new EnemyAttackState(),
            new EnemyPatrolState(),
            new EnemyDeathState(),
            new EnemyHurtState()
        };

        stateMachine.AddGlobalTransition(() => stats.health <= 0 ? stateMachine.GetState<EnemyDeathState>() : null);
        stateMachine.AddGlobalTransition(() =>
        {
            if (pendingHurtStateTransition)
            {
                pendingHurtStateTransition = false; // Reset the flag
                return stateMachine.GetState<EnemyHurtState>();
            }
            return null;
        });


        // Initialize and add states to the state machine
        foreach (var state in states)
        {
            state.Setup(stateMachine, this);
            stateMachine.AddState(state);
        }

            // Optionally, initialize with a default state
            stateMachine.Initialize(stateMachine.GetState<EnemyPatrolState>());


        stats.setHealth();
        stats.setPosture();
    }
        

    void Update()
    {

        stateMachine.Execute();
        PlayerInAttackRange =  CheckPlayerInAttackRange();
        PlayerInDetectionRange = CheckPlayerInDetectionRange();
        MovementDirection();
        currentStateName = currentState?.GetType().Name ?? "None";
    }

    void FixedUpdate()
    {
        stateMachine.FixedExecute();
    }

    private bool CheckPlayerInDetectionRange(){
        if (player == null)
        {
        Debug.LogError("Player reference is missing!");
        return false; // Or handle the situation appropriately
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        return distanceToPlayer <= stats.DetectionRange;
    }

    private bool CheckPlayerInAttackRange()
{
    // Calculate the origin and size to match Gizmos
    Vector2 boxCastOrigin = (Vector2)enemyCollider.bounds.center + new Vector2(transform.localScale.x * colliderDistance, 0);
    Vector2 boxCastSize = new Vector2(enemyCollider.bounds.size.x * attackRange, enemyCollider.bounds.size.y);

    // Perform the BoxCast
    RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0, Vector2.zero, 0, playerLayer);

    // Check if a valid player was hit


    return hit.collider != null;
}



    private void OnDrawGizmos() {
        if (enemyCollider != null)
    {
        // Attack Range: Red WireCube
        Gizmos.color = Color.red;
        Vector3 cubeCenter = enemyCollider.bounds.center 
                             + transform.right * transform.localScale.x * colliderDistance;
        Vector3 cubeSize = new Vector3(attackRange, enemyCollider.bounds.size.y, enemyCollider.bounds.size.z);
        Gizmos.DrawWireCube(cubeCenter, cubeSize);
    }

    // Detection Range: Green WireSphere
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
            player.OnAttackReceived(true, true, 20, stats.PostureDamage);
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

    public void OnAttackReceived(bool canParry, bool canBlock, int HealthDamage, int PostureDamage)
    {
        stats.TakeHealthDamage(HealthDamage);
        stats.TakePostureDamage(PostureDamage);

        if (stats.health > 0) 
        {
            if (currentStateName != "EnemyHurtState") 
            {
            pendingHurtStateTransition = true;
            }
        }
        
    }
}


