using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private SkillTreeSlot_UI dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone On Dash")]
    [SerializeField] private SkillTreeSlot_UI cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arriveal")]
    [SerializeField] private SkillTreeSlot_UI cloneOnArrivalUnlockButton;
    public bool cloneOnCloneOnArrivalUnlocked { get; private set; }
    public override void UseSkill()
    {
        base.UseSkill();
    }
    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);

    }
    protected override void Update()
    {
        base.Update();
        dashUnlocked= dashUnlockButton.unLocked;
        cloneOnDashUnlocked=cloneOnDashUnlockButton.unLocked;
        cloneOnCloneOnArrivalUnlocked=cloneOnArrivalUnlockButton.unLocked;
    }
    #region ½âËø¼¼ÄÜ
    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }
    private void UnlockDash()
    {
        if(dashUnlockButton.unLocked)
        {    
            dashUnlocked = true;    
        }
    }
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unLocked)
        {
            cloneOnDashUnlocked = true;
        }
    }
    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unLocked)
        {
            cloneOnCloneOnArrivalUnlocked = true;
        }
    }
    #endregion
    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked) { SkillManager.instance.clone.CreatClone(player.transform, Vector3.zero); }
    }
    public void CreateCloneOnDashOver()
    {
        if (cloneOnCloneOnArrivalUnlocked) { SkillManager.instance.clone.CreatClone(player.transform, Vector3.zero); }
    }
}
