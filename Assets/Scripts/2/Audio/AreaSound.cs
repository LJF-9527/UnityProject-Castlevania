using System.Collections;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;
    private AudioSource source;
    private float defaultVolume = 1;
    private void Start()
    {
        source = AudioManager.instance.sfx[areaSoundIndex];
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2>() != null)
        {
            if (DecreaseVolume(source) != null)
                StopAllCoroutines();
            AudioManager.instance.PlaySFX(areaSoundIndex);
            IncreaseSFXWithTime();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2>() != null)
        {
            if (DecreaseVolume(source) != null)
                StopAllCoroutines();
            StopSFXWithTime();
        }
    }
    #region ÒôÁ¿½¥±ä
    public void StopSFXWithTime() => StartCoroutine(DecreaseVolume(source));
    public void IncreaseSFXWithTime() => StartCoroutine(IncreaseVolum(source));
    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        
        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.25f);
            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                break;
            }
        }
    }
    private IEnumerator IncreaseVolum(AudioSource _audio)
    {
        while (_audio.volume < .9f)
        {
            _audio.volume += _audio.volume * .2f;
            yield return new WaitForSeconds(.25f);
            if (_audio.volume >= .9f)
            {
                _audio.volume = defaultVolume;
                break;
            }
        }
    }


    #endregion
}
