using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D body;
    public CapsuleCollider2D groundCheck;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    [Header("ScriptableObject Stats")]
    public CharacterStats stats;
    [SerializeField] private float postureRegenRate = 20f; // Amount of posture regenerated per second
    private float postureRegenTimer;
    public bool posturePause;

    [Header("Movement Variables")]
    public bool grounded;
    public float xInput;
    public bool jumpPressed;
    public bool IsMoving => Mathf.Abs(xInput) > 0;

    public int numOfJumps = 1;
    public bool canMove = true;
    [SerializeField] private float coyoteTime = 0.1f; // Time allowed after leaving the ground to still jump
    private float coyoteTimeCounter;

    [Header("State Machine Information")]
    public StateMachine stateMachine;
    public string currentStateName;
    public State currentState => stateMachine?.CurrentState;
    public BlockManager blockManager;
    public bool pendingHurtStateTransition;
    
    
    private void Start()
    {
        // Initialize state machine
        stateMachine = new StateMachine();
        var states = new List<State>
        {
            new IdleState(),
            new WalkState(),
            new AirState(),
            new BasicAttackState(),
            new AirAttackState(),
            new DeathState(),
            new HurtState(),
            new BlockingState()
        };

        stateMachine.AddGlobalTransition(() => stats.health <= 0 ? stateMachine.GetState<DeathState>() : null);
        stateMachine.AddGlobalTransition(() =>
        {
            if (pendingHurtStateTransition)
            {
                pendingHurtStateTransition = false; // Reset the flag
                return stateMachine.GetState<HurtState>();
            }
            return null;
        });

        // Setup states
        foreach (var state in states)
        {
            state.Setup(stateMachine, this);
            stateMachine.AddState(state);
        }

        // Initialize with a default state
        stateMachine.Initialize(stateMachine.GetState<IdleState>());

        stats.health = stats.healthMAX;
        stats.Posture = stats.PostureMAX;
    }

    private void Update()
    {
        CheckInput();
        handleCoyoteTime();
        stateMachine.Execute();
        currentStateName = currentState?.GetType().Name ?? "None";


        RegeneratePosture();
    }

    private void FixedUpdate()
    {
        
        CheckGround();
        MoveWithInput();
        ApplyFriction();
        
        stateMachine.FixedExecute();
    }

    private void RegeneratePosture()
    {
        // Only regenerate posture if it's below the maximum
        if (stats.Posture < stats.PostureMAX && posturePause == false)
        {
            postureRegenTimer += Time.deltaTime;

            // Regenerate posture based on the regen rate
            if (postureRegenTimer >= 1f)
            {
                stats.Posture = Mathf.Min(stats.Posture + postureRegenRate, stats.PostureMAX);
                postureRegenTimer = 0f; // Reset timer after each second
            }
        }
    }
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && (grounded || coyoteTimeCounter >= 0) && numOfJumps > 0)
        {
            jumpPressed = true;
        }
        if (currentState.IsActionState == false && Input.GetMouseButtonDown(1))
        {
            stateMachine.TransitionTo(stateMachine.GetState<BlockingState>());
        }
    }

    private void MoveWithInput()
    {
        if(canMove)
        {
                // Horizontal movement
            if (Mathf.Abs(xInput) > 0)
            {
                float increment = xInput * stats.speed; // Use stats.speed for acceleration
                float newSpeed = Mathf.Clamp(body.velocity.x + increment, -stats.speed, stats.speed);
                body.velocity = new Vector2(newSpeed, body.velocity.y);
                DirectionInput();
            }

            if (jumpPressed)
            {
                Jump();
                numOfJumps--;
                jumpPressed = false;
            }
        }
    }

    private void DirectionInput()
    {
        if (Mathf.Abs(xInput) > 0)
        {

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapArea(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer);
        if(grounded)
        {
            resetJump();
        }
    }

    private void ApplyFriction()
    {
        if (grounded && Mathf.Approximately(xInput, 0f))
        {
            // Apply drag from stats
            body.velocity = new Vector2(body.velocity.x * stats.drag, body.velocity.y);
        }
    }

    private void handleCoyoteTime()
    {
        if (!grounded)
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }
        else
        {
            coyoteTimeCounter = coyoteTime; // Reset when grounded
        }
    }

    private void Jump()
    {
            body.velocity = new Vector2(body.velocity.x, stats.jumpForce); // Use stats.jumpForce
    }
    private void resetJump()
    {
        numOfJumps = 1;
    }


    public int GetHealth()
    {
        return stats.health;
    }
    
    public bool IsCurrentAnimationComplete(string animationName)
    {
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // Check if the current animation matches and if it is complete
    return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }


    public void OnAttackReceived(bool canParry, bool canBlock, int HealthDamage, int PostureDamage)
    {
        blockManager.HandleAttack(canParry, canBlock, HealthDamage, PostureDamage);
    }

    private void OnDrawGizmos()
{
    // Determine the facing direction (-1 for left, 1 for right)
    float facingDirection = transform.localScale.x > 0 ? 1 : -1;

    // Adjust attack area origin based on direction
    Vector2 attackAreaOrigin = (Vector2)transform.position + (Vector2)transform.right * facingDirection * 2 / 2;
    attackAreaOrigin.y += 0.5f;

    // Define attack area size
    Vector2 attackAreaSize = new Vector2(2, 1f);

    // Draw the attack area
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(attackAreaOrigin, attackAreaSize);
}



    private void ApplyDamage(){
        if (stateMachine.CurrentState is BasicAttackState attackState)
        {
            attackState.DetectAndDamageEnemies();  // Now you can call the method
        }
    }

}
