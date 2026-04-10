using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player2 player=>GetComponentInParent<Player2>();
    [SerializeField] private InGame_UI game_UI;

    private  void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2,null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>()!=null&&hit.GetComponent<EnemyStats>().currentHealth>0)
            { 
                EnemyStats _target=hit.GetComponent<EnemyStats>();
                if(_target!=null) 
                player.stats.DoDamage(_target);
                
                if (Inventory.instance.equipmentDictionary.Count != 0)
                { Inventory.instance.GetEquipmentType(EquipmentType.Weapon)?.ExecuteItemEffect(_target.transform); }
            }
            if(hit.GetComponent<OpenChest>()!=null)
            {
                hit.GetComponent<OpenChest>().OpenThisChest();
                player.fX.ScreenShake(player.fX.shakeLowDamage);
                AudioManager.instance.PlaySFX(41);
            }
        }
    }
    private void ThrowSword()
    {
        game_UI.SetCooldownOf(game_UI.swordImage);
        Sword_Skill swordSkills = SkillManager.instance.sword;
        
        swordSkills.CreateSword();
    }
}
