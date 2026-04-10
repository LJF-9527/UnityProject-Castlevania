using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject deletButton;
    [SerializeField] FadeScene_UI fadeScreen;
    
    private void Awake()
    {
        
    }
    private void Start()
    {
        if(!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
            deletButton.SetActive(false);
        }
        
    }
    private void Update()
    {
        if (!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
            deletButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
            deletButton.SetActive(true);
        }
    }
    public void DeletGame()
    {
        SaveManager.instance.DeletSaveData();
    }
    public void ContinueGame()
    {
        
        StartCoroutine(LoadScreenWithFadeEffect(2f));
    }
    public void NewGame()
    {
        SaveManager.instance.DeletSaveData();
        
        StartCoroutine(LoadScreenWithFadeEffect(2f));
    }
    public void ExitGame()
    {
        Debug.Log("ÍË³öÓÎÏ·");
        Application.Quit();
    }
    IEnumerator LoadScreenWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
