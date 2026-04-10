using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/ExecuteItemEffect/Freeze Enemies")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats.currentHealth>playerStats.GetMaxHealthValue()*.5f  ) { return; }
        if (!Inventory.instance.CanUseArmor()) return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
            }
        }
    }
}
