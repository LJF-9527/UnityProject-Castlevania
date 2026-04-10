using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGMPlay : MonoBehaviour
{
    [SerializeField] private int bgmIndex;
    private bool isGrimmBgmPlay7=true;
    private bool isGrimmBgmPlay8;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2>() != null&&isGrimmBgmPlay7&&bgmIndex==7&&!isGrimmBgmPlay8)
        { 
            AudioManager.instance.PlayBGM(bgmIndex);
            isGrimmBgmPlay7 = false;
            isGrimmBgmPlay8 = true;
            Debug.Log(bgmIndex + "play");
        }
        
    }
    private void Update()
    {
        if (!AudioManager.instance.bgm[7].isPlaying&&isGrimmBgmPlay8&&bgmIndex==7&&AudioManager.instance.isPlayBgm)
        {
            isGrimmBgmPlay8 = false;
            bgmIndex++;
            AudioManager.instance.PlayBGM(bgmIndex);
            Debug.Log(bgmIndex + "play");
        }
        
    }
}
