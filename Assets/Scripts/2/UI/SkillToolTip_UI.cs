using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkillToolTip_UI : ToolTips_UI
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowToolTip(string _skillDescription,string _skillName,string _price)
    {
        skillText.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text = "»¨·Ñ£º" + _price+" Souls";
        AdjustPosition();
        AdjustFontSize(skillName);
        gameObject.SetActive(true);
        

    }

    public void HideToolTip() 
    {
        gameObject.SetActive(false);
        SetDefaultSize(skillName); 

    } 
}
