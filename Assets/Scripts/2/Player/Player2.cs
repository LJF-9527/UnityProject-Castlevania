using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Player2 : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    
    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    //[SerializeField] private float dashCooldown;
    //private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    private float defalutDashSpeed;
    public float dashDir { get; private set; }
    #region SkillAbout
    public SkillManager skillManager { get; private set; }
    public GameObject sword { get; private set; }
    #endregion
    #region States
    public PlayerStateMachine stateMachine { get; private set; }//×´Ì¬»ú
    public PlayerIdleState idleState { get; private set; }//ÏÐÖÃ×´Ì¬
    public PlayerMoveState moveState { get; private set; }//ÒÆ¶¯×´Ì¬
    public PlayerJumpState jumpState { get; private set; }//ÌøÔ¾×´Ì¬
    public PlayerAirState airState { get; private set; }//¿ÕÖÐ×´Ì¬
    public PlayerDashState dashState { get; private set; }//³å´Ì
    public PlayerWallSlideState wallSlideState { get; private set; }//»¬Ç½
    public PlayerWallJumpState wallJumpState { get; private set; }//µ¯Ç½Ìø
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }//Ö÷Òª¹¥»÷
    public PlayerCounterAttackState counterAttackState { get; private set; }//·´»÷
    public PlayerAimSwordState aimSwordState { get; private set; }//Í¶ÖÀÎäÆ÷Ç°µÄÃé×¼
    public PlayerCatchSwordState catchSwordState { get; private set; }//»ØÊÕÎäÆ÷Ê±×¥×¡ÎäÆ÷
    public PlayerBlackHoleState blackHoleState { get; private set; }//ºÚ¶´¼¼ÄÜ
    public PlayerDeadState deadState { get; private set; }//ËÀÍö
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        #region AnimSetBool
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this,stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        #endregion
    }
    protected override void Start()
    {
        base.Start();
        skillManager = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defalutDashSpeed=dashSpeed;
    }
    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();
        
        if(Input.GetKeyDown(KeyCode.F)) { skillManager.crystal.CanUseSkill(); }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Inventory.instance.UseFlask(); }
    }
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        base.SlowEntityBy(_slowPercentage, _slowDuration);
        
        moveSpeed = moveSpeed*(1-_slowPercentage);
        jumpForce = jumpForce* (1 - _slowPercentage);
        dashSpeed= dashSpeed*(1 - _slowPercentage);
        anim.speed=anim.speed*(1-_slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defalutDashSpeed;
        
        
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
        rb.drag = 5;
        this.enabled = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    #region SwordAbout
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    #endregion
    
    
    
    private void CheckForDashInput()
    {
        
        if (IsWallDetected()&&!IsGroundDetected()) { SkillManager.instance.dash.cooldownTimer =-1; return; }
        if (!skillManager.dash.dashUnlocked) return;
        
        if (Input.GetKeyDown(KeyCode.LeftShift)&&SkillManager.instance.dash.CanUseSkill()) 
        {
            
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) { dashDir=facingDir; }
            stateMachine.ChangeState(dashState); 
        }
    }
    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
