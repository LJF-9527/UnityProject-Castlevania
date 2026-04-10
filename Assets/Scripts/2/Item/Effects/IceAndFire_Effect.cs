using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceAndFire Effect", menuName = "Data/ExecuteItemEffect/IceAndFire")]

public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceandfirePrefab;
    [Range(3,15)][SerializeField] private float newVelocity;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        base.ExecuteEffect(_respawnPosition);
        Transform player = PlayerManager.instance.player.transform;

        bool thirdAttack = player.GetComponent<Player2>().primaryAttackState.comboCounter == 2;
        if (thirdAttack) 
        {
            GameObject newIceAndFire=Instantiate(iceandfirePrefab, _respawnPosition.position,player.rotation);
        
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = player.right*newVelocity;

            Destroy(newIceAndFire,10);

        }
    }
}
