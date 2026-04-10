using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] public AudioSource[] sfx;
    [SerializeField] public AudioSource[] bgm;

    public bool isPlayBgm;
    private int bgmIndex;

    private bool canPlaySFX;
    
    private void Awake()
    {
        if (instance != null) { Destroy(instance.gameObject); }
        else { instance = this; }
        Invoke("AllowSFX", 1f);
        bgmIndex = 6;
    }
    private void Update()
    {
        if (!isPlayBgm) { StopAllBGM(); }
        else
        {
            if (!bgm[bgmIndex].isPlaying) PlayBGM(bgmIndex);
        }
        
    }

    #region SFX
    private void AllowSFX() => canPlaySFX = true;
    public void PlaySFX(int _sfxIndex, Transform _souce = null)
    {
        //if (sfx[_sfxIndex].isPlaying) { return; }
        if (!canPlaySFX) return;
        if (_souce != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _souce.position) > sfxMinimumDistance)
        { return; }

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.84f, 1.1f);
            sfx[_sfxIndex].Play();
        }
        
    }
    public void StopSFX(int _sfxIndex, float _waittime = 0)
    {
        StartCoroutine("WaitToStopAuio",_waittime);
        sfx[_sfxIndex].Stop();
    }

        private IEnumerator WaitToStopAuio(float _waittime)
    {
        yield return new WaitForSeconds(_waittime);
    }
    #endregion
    #region BGM
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        isPlayBgm = true;
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
            
        }
        isPlayBgm = false;
    }
    public void PlayRandomBGM()
    {
        bgmIndex=Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    #endregion
}
