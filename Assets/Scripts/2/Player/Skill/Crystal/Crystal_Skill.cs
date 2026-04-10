using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    private GameObject currentCrystal;
    [Space]
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;

    [Header("Crystal simple")]
    [SerializeField] private SkillTreeSlot_UI unlockCrystalButton;
    public bool canCreatCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private SkillTreeSlot_UI unlockInsteadCrystalButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private SkillTreeSlot_UI unlockExplosiveButton;
    [SerializeField] private float explosiveCooldown;
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed;

    [Header("Moving Crystal")]
    [SerializeField] private SkillTreeSlot_UI unlockMoveCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private SkillTreeSlot_UI unlockMultiCrystalButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCoolDown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>();
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();

    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) 
        {
            
            return; 
        }

        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy) return;

            Vector2 playerPos=player.transform.position;
            player.transform.position=currentCrystal.transform.position;
            currentCrystal.transform.position=playerPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreatClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }    
    }


    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockInsteadCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosive);
        unlockMoveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockTraceCrystal);
        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }

    protected override void Update()
    {
        base.Update();
        canCreatCrystal = unlockCrystalButton.unLocked;
        cloneInsteadOfCrystal = unlockInsteadCrystalButton.unLocked;
        canExplode= unlockExplosiveButton.unLocked;
        canMoveToEnemy= unlockMoveCrystalButton.unLocked;
        canUseMultiStacks= unlockMultiCrystalButton.unLocked;
    }
    public void CreateCrystal()
    {
        if(!canCreatCrystal) { return; }
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(currentCrystal.transform),player);
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();
    private void RefillCrystal()
    {
        int amountToAdd=amountOfStacks-crystalList.Count;
        for(int i = 0;i<amountToAdd;i++)
        {
            crystalList.Add(crystalPrefab);
        }
    }
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks) 
        {
            if (crystalList.Count > 0)
            {
                if(crystalList.Count==amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                cooldown = 0;
                GameObject crystalToSpawn = crystalList[crystalList.Count-1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                
                crystalList.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalList.Count <= 0)
                {
                    cooldown = multiStackCoolDown;
                    RefillCrystal();
                }
            return true; 
            }
        }
        return false;
    }
    private void ResetAbility()
    {
        if (cooldownTimer > 0) { return; }
        cooldownTimer = multiStackCoolDown;
        RefillCrystal();
    }
    #region ½âËø¼¼ÄÜ
    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockCrystalExplosive();
        UnlockMultiCrystal();
        UnlockTraceCrystal();
    }
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unLocked) { canCreatCrystal = true; }
    }
    private void UnlockCrystalMirage()
    {
        if (unlockInsteadCrystalButton.unLocked) { cloneInsteadOfCrystal = true; }
    }
    private void UnlockCrystalExplosive()
    {
        if (unlockExplosiveButton.unLocked) 
        {
            canExplode = true;
            cooldown = explosiveCooldown;
        }
    }
    private void UnlockTraceCrystal()
    {
        if (unlockMoveCrystalButton.unLocked) { canMoveToEnemy = true; }
    }
    private void UnlockMultiCrystal()
    {
        if (unlockMultiCrystalButton.unLocked) { canUseMultiStacks = true; }
    }
    #endregion
}
