using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player2 player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    private float attackMultiplier;
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius=.8f;
    private Transform closeEnemy;
    private int facingDir=1;

    
    private bool canDuplicateClone;
    private float chanceToDuplicateClone;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        
        cloneTimer-= Time.deltaTime;
        if(cloneTimer<0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if(sr.color.a<=0) { Destroy(this.gameObject); }
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset,Transform _closestEnemy,bool _canDuplicateClone,float _chanceToDuplicate,Player2 _player,float _attackMultiplier)
    {
        if (_offset == null) { _offset = Vector3.zero; }
        if(_canAttack) { anim.SetInteger("AttackNum", Random.Range(1, 3)); }
        transform.position=_newTransform.position+_offset;
        cloneTimer = _cloneDuration;
        closeEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicateClone = _chanceToDuplicate;
        player = _player;
        attackMultiplier= _attackMultiplier;
        FaceClosestTarget();
    }
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                PlayerStats playerStats=player.GetComponent<PlayerStats>();
                EnemyStats enemyStats=hit.GetComponent<EnemyStats>();
                playerStats.CloneDoDamage(enemyStats, attackMultiplier);
                if (player.skillManager.clone.aggresiveCloneUnlocked)
                {
                    if (Inventory.instance.equipmentDictionary.Count != 0)
                    {
                        Inventory.instance.GetEquipmentType(EquipmentType.Weapon)?.ExecuteItemEffect(hit.transform);
                    }
                }
                if(canDuplicateClone)
                {
                    if(Random.Range(0,100)<chanceToDuplicateClone)
                    {
                        SkillManager.instance.clone.CreatClone(hit.transform, new Vector3(1f*facingDir, 0));
                    }
                }
            }
        }
    }
    private void FaceClosestTarget()
    {
        
        if(closeEnemy != null)
        {
            if(transform.position.x>closeEnemy.position.x) 
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0); }
        }
    }
}
