using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Entity : MonoBehaviour
{
    [Header("Konckback info")]
    [SerializeField] protected Vector2 knockbackPower=new Vector2(5,7);
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(0, 2);
    [SerializeField] protected float knockbackDuration=.35f;
    public bool isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius=1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float grounCheckDistance=0.41f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance=.85f;
    [SerializeField] protected LayerMask whatIsGround;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fX { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }  
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    public int knockbackDir { get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        #region 获得组件
        fX = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();  
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
        #endregion
    }
    protected virtual void Update()
    {
        
    }
    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked) { return; }//被击中时不能动
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public virtual void SetZeroVelocity()
    {
        if (isKnocked) { return; }
        if(rb!=null)
        rb.velocity=Vector2.zero;
    }
    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else if(_damageDirection.position.x < transform.position.x)
            knockbackDir = 1;
    }
    public virtual void DamageImpact()=> StartCoroutine("HitKnockback");

    public void SetupKnockbackPower(Vector2 _knockbackPower)=>knockbackPower = _knockbackPower;
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked=true;
        float offset=Random.Range(knockbackOffset.x, knockbackOffset.y);

        if(knockbackPower.x>0||knockbackPower.y>0)
        rb.velocity = new Vector2((knockbackPower.x+offset) * knockbackDir, knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }
    protected virtual void SetupZeroKnockbackPower()
    {

    }
    #region CollisionCheck
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, grounCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - grounCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    #region Flip
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped!=null) 
        onFlipped();
    }
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight) { Flip(); }
        else if (_x < 0 && facingRight) { Flip(); }
    }
    public virtual void SetupDefaultFacingDir(int _dir)
    {
        facingDir = _dir;
        if(facingDir==-1)
        {
            facingRight = false;
        }
        else
            facingRight = true;
    }
    #endregion


    public virtual void Die()
    {

    }
}
