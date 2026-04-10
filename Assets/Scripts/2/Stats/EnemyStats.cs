using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Space]
    [Space]
    [Space]
    
    [Header("¹Ø¿¨ details")]
    [SerializeField] private int level;
    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier=.4f;
    public Stats soulsDropAmount;
    private Enemy enemy;
    private ItemDrop myDropSystem;

    protected override void Start()
    {
        
        ApplyLevelModifier();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }


    private void Modify(Stats _stats)
    {
        for(int i=1;i<level;i++)
        {
            float modifier = _stats.GetValue() * percantageModifier;

            _stats.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }
    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
        DoMagicDamage( _targetStats );
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.amoutOfSouls += soulsDropAmount.GetValue();
        if (this.gameObject.GetComponent<Enemy_Shady>() == null)
        {
            myDropSystem.GenerateDrop();
            
        }

        Destroy(gameObject, 5f);
    }
}
