using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player2 player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player2>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        GameManager.instance.lostSoulsAmount = PlayerManager.instance.amoutOfSouls;
        PlayerManager.instance.amoutOfSouls = 0;
        GetComponent<PlayerItemDrop>().GenerateDrop();
    }
    public override void OnEvasion()
    {
        player.skillManager.dodge.CrateMirageOnDodge();
    }
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if(_damage>GetMaxHealthValue()*.3f)
        {
            player.SetupKnockbackPower(new Vector2(5, 7));
            player.fX.ScreenShake(player.fX.shakeHighDamage);
            int randomSound = Random.Range(34, 35);
            AudioManager.instance.PlaySFX(randomSound);
        }

        ItemData_Equipment currentArmor=Inventory.instance.GetEquipmentType(EquipmentType.Armor);
        if(currentArmor != null) { currentArmor.ExecuteItemEffect(player.transform); }
    }
    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
        { return; }

        int totalDamage = damage.GetValue() + strength.GetValue();
        if(_multiplier > 0) { totalDamage=Mathf.RoundToInt(totalDamage*_multiplier); }

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }
}
