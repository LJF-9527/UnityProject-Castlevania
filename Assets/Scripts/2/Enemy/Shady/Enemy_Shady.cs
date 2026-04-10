using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady info")]
    public int battleMoveSpeedMultiple=3;
    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;
    public ItemDrop myItemDrop;

    #region State
    public ShadyGroundState groundState { get; private set; }
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunState stunState { get; private set; }
    public ShadyDieState dieState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        #region GetAnim
        groundState = new ShadyGroundState(this, stateMachine, "isIdle", this);
        idleState = new ShadyIdleState(this, stateMachine, "isIdle", this);
        moveState = new ShadyMoveState(this, stateMachine, "isMove", this);
        battleState = new ShadyBattleState(this, stateMachine, "isRun", this);
        stunState = new ShadyStunState(this, stateMachine, "isStun", this);
        dieState = new ShadyDieState(this, stateMachine, "isDie", this);
        #endregion
        myItemDrop=GetComponent<ItemDrop>();
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
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieState);
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
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosive=Instantiate(explosivePrefab,attackCheck.position,Quaternion.identity);
        newExplosive.GetComponent<ShadyExplosioneEffect_Controller>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);
        
        cd.enabled = false;
        rb.gravityScale = 0f;
    }
    public void SelfDestroy() =>Destroy(gameObject);
}
