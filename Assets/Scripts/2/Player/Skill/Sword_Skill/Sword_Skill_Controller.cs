using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player2 player;

    private bool canRotate = true;
    private bool isReturning = false;

    private float returnSpeed = 12;
    private float freezeTimeDuration;
    [Header("Pierce info")]
    private float pierceAmount;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float spinDirection;
    private bool onlyOnceGetDuration;

    private float hitTimer;
    private float hitCooldown;

    private float destroyDistance;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        onlyOnceGetDuration = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            { player.CatchTheSword(); }
        }

        BounceLogic();
        SpinLogic();
        //Debug.Log(spinTimer);

    }


    #region Sword_BounceEvent
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count) { targetIndex = 0; }
            }
        }
    }

    public void SetupBounce(bool _isBouncing, int _amoutOfBounce,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amoutOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }
    private void SetupTargetForTarget(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {

            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);

                    }
                }
            }
        }
    }
    #endregion
    #region SpinningEvent
    public void SetupSipn(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    private void SpinLogic()
    {

        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {

                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                hitTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        { SwordSkillDamage(hit.GetComponent<Enemy>()); }
                    }
                }//…À∫¶º‰∏Ù
            }
        }
    }
    private void StopWhenSpinning()
    {
        wasStopped = true;

        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        if (onlyOnceGetDuration == true) { spinTimer = spinDuration; onlyOnceGetDuration = false; }

    }
    #endregion
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    #region ThrowSwordEvent
    public void SetUpSword(Vector2 _dir, float _gravityScale, Player2 _player,float _freezeTimeDuration,float _returenSpeed)
    {
        player = _player;
        rb.velocity = _dir;
        returnSpeed = _returenSpeed;
        rb.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTimeDuration;
        if (pierceAmount <= 0)
        { anim.SetBool("Rotation", true); }
        AudioManager.instance.PlaySFX(27);
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
        transform.Rotate(0, 180, 0);
        
    }
    private void DestroyMe()
    {

        
        Destroy(gameObject);
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) { return; }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }
        SetupTargetForTarget(collision);
        StuckInto(collision);
    }
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing && enemyTarget.Count > 0)
        { return; }
        transform.parent = collision.transform;
        anim.SetBool("Rotation", false);
        AudioManager.instance.PlaySFX(28);
        AudioManager.instance.StopSFX(27);
        GetComponentInChildren<ParticleSystem>().Play();
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats=enemy.GetComponent<EnemyStats>();
        player.stats.DoDamage(enemyStats);
        if (player.skillManager.sword.timeStopUnlocked) { enemy.FreezeTimeFor(freezeTimeDuration); }
        if (player.skillManager.sword.vulnerableUnlocked) { enemyStats.MakeVulnerableFor(freezeTimeDuration); }
        

        ItemData_Equipment equipmentAmulet = Inventory.instance.GetEquipmentType(EquipmentType.Amulet);
        if (equipmentAmulet != null) { equipmentAmulet.ExecuteItemEffect(enemy.transform); }
    }

}
