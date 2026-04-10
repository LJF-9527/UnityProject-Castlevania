using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    private Transform player;

    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] string closestCheckPointId;

    [Header("Lost SoulsInfo")]
    [SerializeField] private GameObject lostSoulsPrefab;
    public int lostSoulsAmount;
    [SerializeField] private float lostPrefabX;
    [SerializeField] private float lostPrefabY;
    private void Awake()
    {
        if (instance != null) { Destroy(instance.gameObject); }
        else { instance = this; }
        checkPoints=FindObjectsOfType<CheckPoint>();
    }
    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }
    public void ReStartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));
    public void SaveData(ref GameData _data)
    {
        _data.lostSoulsAmount=lostSoulsAmount;
        _data.lostPrefabX=player.position.x;
        _data.lostPrefabY=player.position.y;

        if(FindClosestCheckPoint()!=null)
            _data.closestCheckPointId = FindClosestCheckPoint().checkPointId;
        _data.checkPoints.Clear();
        foreach(CheckPoint checkPoint in checkPoints)
        {
            _data.checkPoints.Add(checkPoint.checkPointId, checkPoint.activated);
        }
        
    }
    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale=0;
        else 
            Time.timeScale=1;
    }
    #region 加载
    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);
        LoadCheckPoint(_data);
        PlacePlayerAtClosestCheckPoint(_data);
        LoadLostSouls(_data);
    }
    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            Debug.Log(pair.Key);
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.checkPointId == pair.Key && pair.Value)
                {
                    Debug.Log(checkPoint.checkPointId+" "+pair.Value);
                    checkPoint.ActivateCheckPoint();
                }
            }
        }
    }
    private void LoadLostSouls(GameData _data)
    {
        lostSoulsAmount = _data.lostSoulsAmount;
        lostPrefabX = _data.lostPrefabX;
        lostPrefabY = _data.lostPrefabY;

        if(lostSoulsAmount > 0)
        {
            GameObject newLostSouls = Instantiate(lostSoulsPrefab, new Vector3(lostPrefabX, lostPrefabY), Quaternion.identity);
            newLostSouls.GetComponent<LostSouls>().souls = lostSoulsAmount;
        }
        lostSoulsAmount = 0;
    }
    #endregion
    #region 将玩家放置在最近检查点
    private void PlacePlayerAtClosestCheckPoint(GameData _data)
    {
        if(_data.closestCheckPointId==null) { return; }
        closestCheckPointId = _data.closestCheckPointId;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckPointId == checkPoint.checkPointId)
            {
                Vector3 offset = new Vector3(0, 1f, 0);
                player.position = checkPoint.transform.position + offset;
            }
        }
    }
    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckPoint= null; 
        foreach(var checkPoint in checkPoints)
        {
            float checkPointToCharacter=Vector2.Distance(player.position,checkPoint.transform.position); ;
            if(checkPointToCharacter < closestDistance&&checkPoint.activated)
            {
                closestDistance = checkPointToCharacter;
                closestCheckPoint = checkPoint;
            }
        }
        return closestCheckPoint;
    }
    #endregion
}
