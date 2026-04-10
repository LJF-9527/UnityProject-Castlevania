using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour,ISaveManager
{
    [Header("End Screen")]
    [SerializeField] private FadeScene_UI fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartBTN;
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public SkillToolTip_UI skillToolTip;
    public ItemToolTips_UI itemToolTip;
    public StatToolTips_UI statToolTip;
    public CraftWindow_UI craftWindow;

    

    [SerializeField] private VolumeSlider_UI[] volumSettings;
    private void Awake()
    {
        SwitchTo(skillTreeUI);//修复关闭技能树UI后,启动游戏,技能树失效的bug
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) SwitchWithKeyTo(characterUI);
        if (Input.GetKeyDown(KeyCode.B)) SwitchWithKeyTo(craftUI);
        if(Input.GetKeyDown(KeyCode.K)) SwitchWithKeyTo(skillTreeUI);
        if(Input.GetKeyDown(KeyCode.Escape)) SwitchWithKeyTo(optionsUI);
        
    }
    public void SwitchTo(GameObject _menu)
    {
        for(int i=0;i<transform.childCount;i++)
        {
            bool fadeScreen=transform.GetChild(i).GetComponent<FadeScene_UI>()!=null;   
            if(fadeScreen==false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                AudioManager.instance.StopSFX(7);
            }
        }
        if (_menu != null) 
        {
            AudioManager.instance.PlaySFX(7);
            _menu.SetActive(true); }

        if(GameManager.instance!= null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu!=null&&_menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }
    private void CheckForInGameUI()
    {
        for(int i=0;i<transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf&&transform.GetChild(i).GetComponent<FadeScene_UI>()==null) return;
        }
        SwitchTo(inGameUI);
    }
    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }
    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartBTN.SetActive(true);
    }
    public void ReStartGameButton()=>GameManager.instance.ReStartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,float> pair in _data.volumSettings)
        {
            foreach(VolumeSlider_UI volum in volumSettings)
            {
                if(volum.parametr==pair.Key)
                {
                    volum.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumSettings.Clear();
        foreach(VolumeSlider_UI volum in volumSettings)
        {
            _data.volumSettings.Add(volum.parametr, volum.slider.value);
        }
    }
    public void QuitGam()
    {
        SwitchTo(inGameUI);
        SceneManager.LoadScene("Main Menu");
    }
    
}
