using UnityEngine;
using System.Collections;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private CharacterStats stats;
    private Player2 player;

    [Header("PopUpText")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Screen shake Fx")]
    [SerializeField] private float shakeMultiplier=.15f;
    public Vector3 shakeHighDamage=new Vector3(3,3,0);
    public Vector3 shakeLowDamage=new Vector3(.5f,.5f,0);
    public Vector3 shakeSwordImpact;
    private CinemachineImpulseSource screenShake;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration=.1f;
    [SerializeField] private int flashNum=2;
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particle")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFxPrefab_00;
    [SerializeField] private GameObject hitFxPrefab_01;
    [Space]
    [SerializeField] private ParticleSystem dustFx;

    [Header("After Image Fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    private GameObject myHealthBar;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponentInChildren<CharacterStats>();
        screenShake=GetComponent<CinemachineImpulseSource>();
        player=PlayerManager.instance.player;
        originalMat = sr.material;
        myHealthBar = GetComponentInChildren<HealthBar_UI>().gameObject;
    }
    private void Update()
    {
        afterImageCooldownTimer-=Time.deltaTime;
    }
    public void CreatePopUpText(string _text,Color _color)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY,0);
        Entity entity=GetComponent<Entity>();
        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().color = _color;
        newText.GetComponent<TextMeshPro>().text = _text;
    }//Œƒ±æÃ· æ
    public void ScreenShake(Vector3 _shakePower)//æµÕ∑∂∂∂Ø
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
        
    }
    public void CreateAfterImage()
    {
        if(afterImageCooldownTimer<0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage=Instantiate(afterImagePrefab,transform.position,transform.rotation);

            newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }//÷Õ¡Ùª√”∞
    public void PlayDustFX()
    {
        if (dustFx != null) { dustFx.Play(); }
    }//ª“≥æ
    public void CreateHitFx(Transform _target,bool _critical)
    {

        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFxPrefab_00;
        if(_critical) 
        { 
            hitPrefab = hitFxPrefab_01;
            float yRotation = 0;
            zRotation = Random.Range(-45, 45);
            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;
            hitFxRotation=new Vector3(0, yRotation, zRotation);   
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position+new Vector3(xPosition,yPosition,0), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, .5f);
    }//√¸÷–∑¥¿°
    #region FXAbout
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent) 
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear; }
        else 
        { 
            myHealthBar.SetActive(true);
            sr.color = Color.white; }
        
    }
    private IEnumerator FlashFX()
    {
        if (stats.isChilled || stats.isShocked || stats.isIgnited)
        {
            sr.material = hitMat;
            Color currentColor = sr.color;
            sr.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            sr.color = currentColor;
            sr.material = originalMat;
        }
        else
        {
            for (int i = 0; i < flashNum; i++)
            {
                yield return new WaitForSeconds(flashDuration);
                sr.material = hitMat;
                Color currentColor = sr.color;
                sr.color = Color.white;
                yield return new WaitForSeconds(flashDuration);
                sr.color = currentColor;
                sr.material = originalMat;
            }
        }
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white) { sr.color = Color.white; }
        else { sr.color = Color.red; }
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }
    #endregion

    #region µ„»º◊¥Ã¨fx
    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFx()
    {

        if (sr.color != igniteColor[0]) { sr.color = igniteColor[0]; }
        else { sr.color = igniteColor[1]; }
    }
    #endregion

    #region ¬È±‘◊¥Ã¨fx
    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0]) { sr.color = shockColor[0]; }
        else { sr.color = shockColor[1]; }
    }
    #endregion

    #region ±˘∂≥◊¥Ã¨fx
    public void ChillFxFor(float _seconds)
    {
        chillFx.Play();

        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void ChillColorFX()
    {
        if (sr.color != chillColor[0]) { sr.color = chillColor[0]; }
        else { sr.color = chillColor[1]; }
    }
    #endregion

}
