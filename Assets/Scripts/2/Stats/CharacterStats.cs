using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    damage,
    critChance,
    critPower,
    fireDamage,
    iceDamage,
    lightingDamage,
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    [Tooltip("当前生命值")]
    public int currentHealth;//当前生命值
    [Header("主要 stats")]
    public Stats strength;//力量%，影响基础伤害与暴击伤害
    public Stats agility;//敏捷%，影响暴击和闪避s
    public Stats intelligence;//智力，百分比增加魔法伤害与魔法抵抗%
    public Stats vitality;//体力%

    [Header("防御 stats")]
    public Stats maxHealth;//最大生命值
    public Stats armor;//护甲
    public Stats evasion;//闪避
    public Stats magicResistance;//魔法抵抗力

    [Header("伤害 stats")]
    public Stats damage;//当前伤害
    public Stats critChance;//暴击几率
    public Stats critPower;//暴击伤害,默认150%

    [Header("魔法 stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    [Header("异常状态")]
    public bool isIgnited;//是否被点燃；持续受伤
    public bool isChilled;//是否被冰;减抗20%
    public bool isShocked;//是否被电；20%挥空与增大被击概率；麻痹，影响敏捷 

    [SerializeField] private float ailmentsDuration=4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTiemr;

    private float igniteDamageCooldown = .5f;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;

    public System.Action onHealthChange;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    [Header("格挡状态")]
    [SerializeField] private float blockPercentage=0.5f;
    public bool isBlock { get; private set; }
    private bool isVulnerable;
    private bool isCrit;
    protected virtual void Start()
    {
        critPower.SetDefalutValue(150);
        currentHealth = GetMaxHealthValue();
        fx=GetComponent<EntityFX>();
    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTiemr -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0) isIgnited = false;
        if (chilledTimer < 0) isChilled = false;
        if (chilledTimer < 0) isChilled = false;
        if (shockedTiemr < 0) isShocked = false;
        if(isIgnited) ApplyIgniteDamage();

    }
    public virtual void IncreaseStatBy(int _modifier,float _duration,Stats _statToModifiy)
    {
        StartCoroutine(StatModCoroutine(_modifier,_duration,_statToModifiy));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statToModifiy)
    {
        _statToModifiy.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModifiy.RemoveModifier(_modifier);
    }
    #region Vulnerable
    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerabledForCoroutine(_duration));
    }
    private IEnumerator VulnerabledForCoroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable=false;
    }
    #endregion
    #region Strength
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike=false;

        if (TargetCanAvoidAttack(_targetStats)||_targetStats.isInvincible)
        { return; }

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage=CalculateCriticalDamage(totalDamage);
            criticalStrike=true;
            
        }
        

        fx.CreateHitFx(_targetStats.transform,criticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        if (_targetStats.isBlock)
        { 
            totalDamage=Mathf.RoundToInt(totalDamage* blockPercentage);
            AudioManager.instance.PlaySFX(0);
        }
        _targetStats.TakeDamage(totalDamage);
        //DoMagicDamage(_targetStats);
    }
    
    #endregion
    #region Magic&Ailment
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _firDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _firDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_firDamage, _iceDamage, _lightingDamage) <= 0) return;
        AttemptToApplyAilment(_targetStats, _firDamage, _iceDamage, _lightingDamage);

    }
    private static void AttemptToApplyAilment(CharacterStats _targetStats, int _firDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _firDamage > _iceDamage && _firDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _firDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _firDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .5f && _firDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_firDamage * .2f));

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite&& canApplyIgnite)
        {
            isIgnited=_ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }
        if(_chill&& canApplyChill)
        {
            isChilled=_chill;
            chilledTimer = ailmentsDuration;

            fx.ChillFxFor(ailmentsDuration);

            float slowPercentage=.5f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
        }
        if (_shock&& canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player2>() != null) return;

                HitNearestTargetWithShockStrike();
            }
        } 
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 )
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0&&!isDead) Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked) { return; }

        isShocked = _shock;
        shockedTiemr = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        //寻找最近敌人
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 25);
        float closesetDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closesetDistance)
                {
                    closesetDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null) { closestEnemy = transform; }
        }
        //生成闪电预制体
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage)=>igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage)=>shockDamage = _damage;
    #endregion
    #region Crit
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritDamage = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritDamage;
        return Mathf.RoundToInt(critDamage);
    }
    protected bool CanCrit()
    {
        int totalCriticalChance=critChance.GetValue()+agility.GetValue();
        if(Random.Range(0,100)<=totalCriticalChance)
        {
            isCrit = true;
            return true;
        }
        isCrit = false;
        return false;
    }
    #endregion
    #region Stats Calculations
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }
    protected  bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked) totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    protected static int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);//限制totalDamage范围
        return totalDamage;
    }

    #endregion
    #region Foundation
    public virtual void OnEvasion()
    {

    }
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if(currentHealth>=GetMaxHealthValue())currentHealth=GetMaxHealthValue();
        if(onHealthChange != null) onHealthChange();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if(isVulnerable) { _damage = Mathf.RoundToInt(_damage * 1.2f); }

        currentHealth -= _damage;

        if (_damage >0)
        {
                fx.CreatePopUpText(_damage.ToString(), Color.white);
            
        }

        if (onHealthChange != null) onHealthChange();

        if (_damage > GetMaxHealthValue() * .3f)
        {
            fx.ScreenShake(fx.shakeHighDamage);
        }
        else
            fx.ScreenShake(fx.shakeLowDamage);
    }
    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible) return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        AudioManager.instance.PlaySFX(40);
        if (currentHealth <= 0&&!isDead)
        {
            Die();
        }
        onHealthChange();
    }
    protected virtual void Die()
    {
        isDead = true;
    }
    public void KillEntity()
    {
        if (!isDead) { Die(); }
    }
    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue();
    public void MakeInvincible(bool _invincible)=>isInvincible = _invincible;
    public void MakeBlock(bool _block)=>isBlock = _block;
    #endregion
    public Stats GetStats(StatType _statType)
    {
        if (_statType == StatType.strength) { return strength; }
        else if (_statType == StatType.agility) { return agility; }
        else if (_statType == StatType.intelligence) { return intelligence; }
        else if (_statType == StatType.vitality) { return vitality; }
        else if (_statType == StatType.maxHealth) { return maxHealth; }
        else if (_statType == StatType.armor) { return armor; }
        else if (_statType == StatType.evasion) { return evasion; }
        else if (_statType == StatType.magicResistance) { return magicResistance; }
        else if (_statType == StatType.damage) { return damage; }
        else if (_statType == StatType.critChance) { return critChance; }
        else if (_statType == StatType.critPower) { return critPower; }
        else if (_statType == StatType.fireDamage) { return fireDamage; }
        else if (_statType == StatType.iceDamage) { return iceDamage; }
        else if (_statType == StatType.lightingDamage) { return lightingDamage; }

        return null;
    }
}
