using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    public GameData gameData;
    private List<ISaveManager> isaveManagers;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if(instance!=null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName,encryptData);
        isaveManagers = FindAllSaveManager();
        LoadGame();
    }
    [ContextMenu("删除存档文件")]
    public void DeletSaveData()
    {
        dataHandler=new FileDataHandler(Application.persistentDataPath,fileName, encryptData);
        dataHandler.Delet();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("没找到存档数据，正在新建");
            NewGame();
        }
        foreach(ISaveManager isaveManager in isaveManagers)
        {
            isaveManager.LoadData(gameData);
        }
        Debug.Log("加载游戏");
    }

    public void SaveGame()
    {
        foreach(ISaveManager isaveManager in isaveManagers)
        {
            isaveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
        
        Debug.Log("游戏已保存");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> isaveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(isaveManagers);
    }
    public bool HasSaveData()
    {
        if(dataHandler.Load()!=null)
        {
            return true;
        }
        return false;
    }
}
