using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge info")]
    [SerializeField] private SkillTreeSlot_UI unlockDodgeButton;
    [Range(0f, 1f)][SerializeField] private float agilityIncreasePercentage;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private SkillTreeSlot_UI unlockDodgeMirageButton;
    public bool dodgeMirageUnlocked;
    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockDodgeMirageButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }
    protected override void Update()
    {
        base.Update();
        dodgeUnlocked = unlockDodgeButton.unLocked;
        dodgeMirageUnlocked = unlockDodgeMirageButton.unLocked;
    }
    #region ½âËø¼¼ÄÜ
    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockDodgeMirage();
    }
    private void UnlockDodge()
    {
        if (unlockDodgeButton.unLocked) 
        {
            int increaseAmount = Mathf.RoundToInt(player.stats.agility.GetValue() * agilityIncreasePercentage);
            player.stats.agility.AddModifier(increaseAmount);
            
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true; 
        }
    }
    private void UnlockDodgeMirage()
    {
        if (unlockDodgeMirageButton.unLocked) { dodgeMirageUnlocked = true; }
    }
    #endregion
    public void CrateMirageOnDodge()
    {
        if (!dodgeMirageUnlocked) { return; }
        SkillManager.instance.clone.CreatClone(player.transform,new Vector3(2*player.facingDir,0));
    }
}
