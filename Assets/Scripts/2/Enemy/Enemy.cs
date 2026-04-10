using Cinemachine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Stun info")]
    public float stunDuration=.5f;
    public Vector2 stuDirection=new Vector2(7,5);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed=1.5f;
    public float idleTime=2;
    public float battleTime=3;
    private float defaultMoveSpeed;
    [Header("Attack info")]
    public float attackDistance=1.5f;
    public float attackCooldown=.4f;
    public float maxAttackCooldown=.6f;
    public float minAttackCooldown=.2f;
    public float alertDistance = 2;
    [HideInInspector] public float lastTimeAttacked;
    
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    #region ±ù¶³×´Ì¬
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        base.SlowEntityBy(_slowPercentage, _slowDuration);

        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed=anim.speed*(1-_slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed=defaultMoveSpeed;
    }
    #endregion
    public virtual void AssignLastAnimName(string _animBoolName)=> lastAnimBoolName = _animBoolName;
    #region ÔÒ£¡ÍßÂ³¶à£¡//µÐÈËÊÜ»÷Í£Ö¹
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0f;
            anim.speed = 0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1f;
        }
    }
    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));
    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);  
        FreezeTime(false);
    }
    #endregion
    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned=true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion
    public virtual bool CanBeStunned()
    {
        if (canBeStunned) { CloseCounterAttackWindow(); return true; }
        return false;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x+attackDistance*facingDir,transform.position.y));
        
    }
    public override void Die()
    {
        base.Die();
        
    }
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 12, whatIsPlayer);
    public virtual void AnimationFinishTrigger()=>stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialAttackTrigger()
    {

    }
}
