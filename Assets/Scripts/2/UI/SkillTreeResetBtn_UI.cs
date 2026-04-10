using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeResetBtn_UI : MonoBehaviour
{
    public SkillTreeSlot_UI[] skillSlot;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetSkillTree()
    {
        for(int i = 0; i < skillSlot.Length; i++)
        {
            if (skillSlot[i] != null && skillSlot[i].unLocked)
            {
                skillSlot[i].unLocked = false;
                PlayerManager.instance.amoutOfSouls += skillSlot[i].skillPrice;
            }
        }
       // SkillManager.instance.parry.parryUnlocked=false;
    }
}
