using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("反击")]
    [SerializeField] private SkillTreeSlot_UI parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("反击汲取")]
    [SerializeField] private SkillTreeSlot_UI restoreUnlockButton;
    [Range(0f, 1f)][SerializeField] private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("反击援助")]
    [SerializeField] private SkillTreeSlot_UI parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);
    }
    protected override void Update()
    {
        base.Update();
        parryUnlocked = parryUnlockButton.unLocked;
        restoreUnlocked= restoreUnlockButton.unLocked;
        parryWithMirageUnlocked = parryWithMirageUnlockButton.unLocked;
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }
    #region 解锁技能
    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryMirage();
    }
    private void UnlockParry()
    {
        if (parryUnlockButton.unLocked) { parryUnlocked = true; }
    }
    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unLocked) { restoreUnlocked = true; }
    }
    private void UnlockParryMirage()
    {
        if (parryWithMirageUnlockButton.unLocked) { parryWithMirageUnlocked = true; }
    }
    #endregion
    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked) { SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform); }
    }
}
