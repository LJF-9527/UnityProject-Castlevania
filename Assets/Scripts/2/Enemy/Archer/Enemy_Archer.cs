using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{

    [Header("Archer special info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed=10;

    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance;//玩家多近时才逃跑后跳
    [HideInInspector] public float lastTimeJumped;
    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;
    [Space]
    [SerializeField] private float wallBehindCheckDistance;

    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunState stunState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        #region GetAnim
        idleState = new ArcherIdleState(this,stateMachine, "isIdle", this);
        moveState = new ArcherMoveState(this,stateMachine, "isMove", this);
        battleState = new ArcherBattleState(this, stateMachine, "isIdle", this);
        attackState = new ArcherAttackState(this, stateMachine, "isAttack", this);
        stunState = new ArcherStunState(this, stateMachine, "isStun", this);
        deadState = new ArcherDeadState(this, stateMachine, "isDie", this);
        jumpState = new ArcherJumpState(this, stateMachine, "isJump", this);
        #endregion
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

    }
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDir, stats);
    }
    public bool GroundBehindeCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right*-1, wallBehindCheckDistance,whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallBehindCheckDistance * -facingDir, wallCheck.position.y));
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }
}
