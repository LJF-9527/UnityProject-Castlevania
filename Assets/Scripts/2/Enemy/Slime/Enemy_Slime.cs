using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum SlimeTyep { big,medium,small}
public class Enemy_Slime : Enemy
{
    [Header("Slime spesific")]
    [SerializeField] public SlimeTyep slimeTyep;
    [SerializeField] private int silmesToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;


    #region State
    public SlimeIdleState idleState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeGroundState groundState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunState stunState { get; private set; }
    public SlimeDieState dieState { get; private set; }
    public SlimeRespawnState respawnState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        #region GetAnim
        idleState = new SlimeIdleState(this, stateMachine, "isIdle", this);
        groundState = new SlimeGroundState(this, stateMachine, "isIdle", this);
        moveState = new SlimeMoveState(this, stateMachine, "isMove", this);
        battleState = new SlimeBattleState(this, stateMachine, "isMove", this);
        attackState = new SlimeAttackState(this, stateMachine, "isAttack", this);
        stunState = new SlimeStunState(this, stateMachine, "isStun", this);
        dieState = new SlimeDieState(this, stateMachine, "isDie", this);
        respawnState = new SlimeRespawnState(this, stateMachine, "isRespawn", this);
        #endregion
        SetupDefaultFacingDir(-1);

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
    private void CreateSlimes(int _amountOfSlimes,GameObject _slimePrefab)
    {
        for(int i =0;i<_amountOfSlimes;i++)
        {
            GameObject newSlime=Instantiate(_slimePrefab,transform.position,Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingDir); 
        }
    }
    public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDir) Flip();

        float xVelocity=Random.Range(minCreationVelocity.x,maxCreationVelocity.x);
        float yVelocity=Random.Range(minCreationVelocity.y,maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * Random.Range(-1,1), yVelocity);

        Invoke("CancleKnockback", 1.5f);
    }
    private void CancleKnockback()=>isKnocked = false;
    public override void Die()
    {
        base.Die();
        
        stateMachine.ChangeState(dieState);
        if (slimeTyep == SlimeTyep.small)
        { return; }
        CreateSlimes(silmesToCreate, slimePrefab);
        
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
