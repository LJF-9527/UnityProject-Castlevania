using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    [Space(20)]
    public bool bossFigherBegun;

    [Header("Teleport info")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport=20;
    [Space]
    [Header("SpellCast info")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells;
    public float castCooldown;
    [SerializeField] private float spellStateCooldown;
    public float lastTimeCast;
    

    #region State
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringenBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);
        #region GetAnim
        idleState = new DeathBringerIdleState(this, stateMachine, "isIdle", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "isTeleport", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "isSpellCast", this);
        battleState = new DeathBringenBattleState(this, stateMachine, "isMove", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "isAttack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "isIdle", this);
        #endregion
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
        
    }
    public void CastSpell()
    {
        Player2 player = PlayerManager.instance.player;
        float xOffset = 0;
        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * 3;
        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 1.5f);
        GameObject newSpell = Instantiate(spellPrefab,spellPosition,Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
    }
    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x+3,arena.bounds.max.x-3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);
        transform.position=new Vector3(x,y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));
        
        if(!GroundBelow()||SomethingIsAround())
        {
            FindPosition();
        }
    }
    private RaycastHit2D GroundBelow()=>Physics2D.Raycast(transform.position,Vector2.down,100,whatIsGround);
    private bool SomethingIsAround()=>Physics2D.BoxCast(transform.position,surroundingCheckSize,0,Vector2.zero,0,whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
    public bool CanTeleport()
    {
        if(Random.Range(0,100)<=chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        return false;
    }
    public bool CanSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            
            
            return true;
        }
        else { return false; }
    }
}
