using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    private EnemyStats myStats;
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunState stunState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        
        #region GetAnim
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunState = new SkeletonStunState(this,stateMachine, "Stun", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this);
        #endregion
    }

    protected override void Start()
    {
        base.Start();
        myStats=GetComponent<EnemyStats>();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        
        
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
    public override bool CanBeStunned()
    {
        if(base.CanBeStunned()&&SkillManager.instance.parry.cooldownTimer<0)
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }
}
