using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string checkPointId;
    public bool activated;

    private void Awake()
    {
        
         anim = GetComponent<Animator>();
    }
    private void Start()
    {
    }
    [ContextMenu("生成检查点ID")]
    private void GenerateId()
    {
        checkPointId=System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2>() != null)
        {
            ActivateCheckPoint();
            SaveManager.instance.SaveGame();
        }
    }

    public void ActivateCheckPoint()
    {
        if (!activated) { AudioManager.instance.PlaySFX(5, transform); }
        
        activated = true;
        anim.SetBool("isActive", true);
    }
}
