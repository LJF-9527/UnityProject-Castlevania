using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;
    public Player2 player;

    public int amoutOfSouls;
    private void Awake()
    {
        if (instance != null) { Destroy(instance.gameObject); }
        else { instance = this; }        
    }
    public bool HaveEnoughMoney(int _price)
    {
        if(_price>amoutOfSouls)
        {
            return false;
        }
        amoutOfSouls=amoutOfSouls-_price; 
        return true;
    }
    public int CurrentCurrencyAmount() => amoutOfSouls;

    public void LoadData(GameData _data)
    {
        this.amoutOfSouls = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.amoutOfSouls;
    }
}
