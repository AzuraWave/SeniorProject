using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
public class PlayerController : MonoBehaviour, IDamageable
{
    public Animator animator;
    public Rigidbody2D body;
    public CapsuleCollider2D groundCheck;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public PlayerObjectSpawner objectSpawner;
    public InputHandler inputHandler;

    [Header("ScriptableObject Stats")]
    public CharacterStats stats;
    [SerializeField] private float postureRegenRate = 20f; // Amount of posture regenerated per second
    private float postureRegenTimer;
    public bool posturePause;

    public bool invulnerable;

    [Header("Movement Variables")]
    private bool throwKeyPressed;
    public bool isAlive;
    public float currentSpeed;
    public bool grounded;
    public float xInput;
    public bool jumpPressed;
    public bool IsMoving => Mathf.Abs(xInput) > 0;
    public bool IsBlocking;
    public int numOfJumps = 1;
    public bool canMove = true;
    public bool isRunning;
    public bool canDash;
    public bool isDashing;
    public float dashingPower = 60f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool isKnockedBack;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    [Header("State Machine Information")]
    public StateMachine stateMachine;
    public string currentStateName;
    public State currentState => stateMachine?.CurrentState;
    public BlockManager blockManager;
    public bool pendingHurtStateTransition;

    public Rigidbody2D rb;

    
    
    private void Start()
    {
        isAlive = true;
        rb = GetComponent<Rigidbody2D>();
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
            new BlockingState(),
            new ThrowingState(),
            new DashState(),
            new RunState()
        };

        stateMachine.AddGlobalTransition(() => {
            if (stats.health <= 0)
            {
                isAlive = false;
                return stateMachine.GetState<DeathState>();
            }
            return null;
        });
        stateMachine.AddGlobalTransition(() =>
        {
            if (pendingHurtStateTransition)
            {
                pendingHurtStateTransition = false; // Reset the flag
                return stateMachine.GetState<HurtState>();
            }
            return null;
        });
        stateMachine.AddGlobalTransition(() =>
        {
            if (grounded && throwKeyPressed && currentState.IsActionState == false)
            {
                throwKeyPressed = false; // Reset after transitioning
                return stateMachine.GetState<ThrowingState>();
            }
            return null;
        });


        
        foreach (var state in states)
        {
            state.Setup(stateMachine, this);
            stateMachine.AddState(state);
        }

       
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
      
        if (stats.Posture < stats.PostureMAX && posturePause == false)
        {
            postureRegenTimer += Time.deltaTime;

            // Regenerate posture based on the regen rate
            if (postureRegenTimer >= 1f)
            {
                stats.Posture = Mathf.Min(stats.Posture + postureRegenRate, stats.PostureMAX);
                postureRegenTimer = 0f; 
            }
        }
    }
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && (grounded || coyoteTimeCounter > 0) && numOfJumps > 0)
        {
            jumpPressed = true;
        }
        if (currentState.IsActionState == false && Input.GetMouseButtonDown(1) && stats.Posture > 0)
        {
            stateMachine.TransitionTo(stateMachine.GetState<BlockingState>());
        }
    }


    private void MoveWithInput()
    {
        if (isDashing)
        {
            return;
        }
        if(canMove && isAlive)
        {
            if(grounded){
                currentSpeed = isRunning ? stats.runningSpeed : stats.speed; 
            }
            

            if (Mathf.Abs(xInput) > 0)
            {
                float increment = xInput * currentSpeed;      
                body.velocity = new Vector2(increment, body.velocity.y);
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
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapArea(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer);
        if(grounded)
        {
            if(!wasGrounded)
            {
                SFXManager.Instance.PlaySound2D("Landing");
            }
            resetJump();
        }
    }

    private void ApplyFriction()
    {
        if (grounded && Mathf.Approximately(xInput, 0f) && !isDashing)
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
            coyoteTimeCounter = coyoteTime; 
        }
    }

    private void Jump()
    {
            SFXManager.Instance.PlaySound2D("Jumping");
            body.velocity = new Vector2(body.velocity.x, stats.jumpForce); 
    }
    private void resetJump()
    {
        numOfJumps = stats.MAXnumOfJumps;
    }


    public int GetHealth()
    {
        return stats.health;
    }
    
    public bool IsCurrentAnimationComplete(string animationName)
    {
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    
    return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }


    public void OnAttackReceived(AttackData attackData)
    {
        if (invulnerable)
        {
            return;
        }
        blockManager.HandleAttack(attackData);
    }

    public void PausePostureCoroutine()
    {
        StartCoroutine(PausePosture(2f));
    }
    public IEnumerator PausePosture(float duration)
    {
        posturePause = true;
        yield return new WaitForSeconds(duration);
        posturePause = false;
    }
    private void OnDrawGizmos()
{
    
    float facingDirection = transform.localScale.x > 0 ? 1 : -1;

    
    Vector2 attackAreaOrigin = (Vector2)transform.position + (Vector2)transform.right * facingDirection * 2 / 2;
    attackAreaOrigin.y += 0.5f;

    
    Vector2 attackAreaSize = new Vector2(2, 1f);

    
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(attackAreaOrigin, attackAreaSize);
}



    private void ApplyDamage(){
        if (stateMachine.CurrentState is BasicAttackState attackState)
        {
            attackState.DetectAndDamageEnemies(); 
        }
    }
    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(KnockbackRecovery());
        }
    }

    private IEnumerator KnockbackRecovery()
    {
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero; 
        isKnockedBack = false;
    }
}
