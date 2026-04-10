using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    [HideInInspector] public float cooldownTimer; 
    protected Player2 player;

    protected virtual void Start()
    {
        player=PlayerManager.instance.player;
        CheckUnlock();
    }

    protected virtual void Update()
    {
        cooldownTimer-=Time.deltaTime;
    }
    protected virtual void CheckUnlock()
    {

    }
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        player.fX.CreatePopUpText("¼¼ÄÜCD:"+cooldownTimer.ToString("f1"), Color.white);
        return false;
    }
    public virtual void UseSkill()
    {

    }
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closesetDistance = Mathf.Infinity;
        
        Transform closestEnemy=null;
        foreach (var hit in collider)
        {
            
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closesetDistance)
                {
                    closesetDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        
        return closestEnemy;
    }
    
}
