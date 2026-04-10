using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [SerializeField] private SkillTreeSlot_UI blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }
    [Space]
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    BlackHole_Skill_Controller currentBlackHole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
        
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackHole.SetUpBlackHole(maxSize, growSpeed,shrinkSpeed,amountOfAttack,cloneCooldown,blackholeDuration);
        
        AudioManager.instance.PlaySFX(6, player.transform);
    }
    
    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlakHole);
    }

    protected override void Update()
    {
        base.Update();
        blackHoleUnlocked = blackHoleUnlockButton.unLocked;
    }
    public bool BlackHoleFinished()
    {
        if(!currentBlackHole)return false;
        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole=null;
            return true;
        }
        return false;
    }
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
    #region ½âËø¼¼ÄÜ
    protected override void CheckUnlock()
    {

        UnlockBlakHole();
    }
    private void UnlockBlakHole()
    {
        if(blackHoleUnlockButton.unLocked) { blackHoleUnlocked = true; }
    }
    #endregion
}
