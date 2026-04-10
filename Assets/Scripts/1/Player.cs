using UnityEngine;

public class Player : Entity1
{
    private float xInput;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    private float dashTime;
    private float dashCooldownTimer;

    [Header("Attack info")]
    [SerializeField] private float comboTime=.3f;
    private float comboTimeWindow;
    private bool isAttacking=false;
    private int comboCounter;

    

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow-=Time.deltaTime;

        

        CheckInput();
        Movement();
        AnimatorControllers();
        FlipController();
    }
    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;
        if(comboCounter>2) { comboCounter=0; }
        
    }


    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.K))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            DashAbility();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            StartAttackEvent();
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded) return;
        if (comboTimeWindow < 0) { comboCounter = 0; }
        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0&&!isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }


    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }

    }

    private void Jump()
    {
        if (isGrounded) { rb.velocity = new Vector2(rb.velocity.x, jumpForce); }

    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isJump", !isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }
    
    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

}
