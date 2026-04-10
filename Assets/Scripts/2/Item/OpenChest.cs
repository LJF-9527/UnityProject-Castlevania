using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    private ItemDrop myItemDrop;
    private Animator anim;

    private void Start()
    {
        myItemDrop=GetComponent<ItemDrop>();
        anim = GetComponent<Animator>();
    }
    public void OpenThisChest()
    {
        anim.SetBool("isOpen", true);
        
        
        Destroy(gameObject,5f);
    }
    private void itemdrop()
    {
        myItemDrop.GenerateDrop();
    }
}
