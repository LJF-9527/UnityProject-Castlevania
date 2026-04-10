using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSouls : MonoBehaviour
{
    public int souls;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2>() != null)
        {
            PlayerManager.instance.amoutOfSouls = souls;
            Destroy(this.gameObject);
        }
    }
}
