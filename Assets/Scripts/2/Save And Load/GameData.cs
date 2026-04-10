using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, bool> skillTrees;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> checkPoints;
    public List<string> equipmentId;

    public string closestCheckPointId;

    public float lostPrefabX;
    public float lostPrefabY;
    public int lostSoulsAmount;

    public SerializableDictionary<string,float> volumSettings;
    public GameData()
    {
        this.lostPrefabX = 0;
        this.lostPrefabY = 0;
        this.lostSoulsAmount = 0;
        this.currency = 0;
        skillTrees = new SerializableDictionary<string, bool>();
        inventory=new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkPoints= new SerializableDictionary<string, bool>();    
        closestCheckPointId=string.Empty;

        volumSettings = new SerializableDictionary<string, float>();
    }
}
