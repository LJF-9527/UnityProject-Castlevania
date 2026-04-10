using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Stats 
{
    [SerializeField] private int 数值;

    public List<int> 加成;

    
    public int GetValue()
    {
        int finalValue=数值;
        foreach(int modifier in 加成)
        {
            finalValue += modifier;
        }
        return finalValue;
    }
    public void SetDefalutValue(int _value)
    {
        数值 = _value;
    }
    public void AddModifier(int _modifier)
    {
        加成.Add(_modifier);
    }
    public void RemoveModifier(int _modifier)
    {
        加成.Remove(_modifier);
    }
}
