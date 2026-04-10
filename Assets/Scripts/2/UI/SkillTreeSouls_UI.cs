using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeSouls_UI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSoulsUI();    
    }
    private void UpdateSoulsUI()
    {
        
        soulsAmount = PlayerManager.instance.CurrentCurrencyAmount();
        currentSouls.text = ((int)soulsAmount).ToString();
        
    }
}
