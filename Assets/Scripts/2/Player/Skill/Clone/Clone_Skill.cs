using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float attackMultiplier;
    [Space]
    [Header("Clone attack")]
    [SerializeField] private SkillTreeSlot_UI cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;
    [Header("Aggresive clone")]
    [SerializeField] private SkillTreeSlot_UI aggresiveCloneUnlockButton;
    public bool aggresiveCloneUnlocked { get;private set; }
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    [Header("Duplicate Clone")]
    [SerializeField] private SkillTreeSlot_UI duplicateCloneUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Header("CreateCrystalClone")]
    [SerializeField] private SkillTreeSlot_UI crystalCloneUnlockButton;
    [SerializeField] public bool isCrystalClone;
    protected override void Start()
    {
        base.Start(); 
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAttackClone);
        duplicateCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDuplicateClone);
        crystalCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalClone);
    }
    protected override void Update()
    {
        canAttack = cloneAttackUnlockButton.unLocked;
        aggresiveCloneUnlocked= aggresiveCloneUnlockButton.unLocked;
        canDuplicateClone=duplicateCloneUnlockButton.unLocked;
        isCrystalClone=crystalCloneUnlockButton.unLocked;
    }
    #region ½âËø¼¼ÄÜ
    protected override void CheckUnlock()
    {
        UnlockAttackClone();
        UnlockAggresiveClone();
        UnlockCrystalClone();
        UnlockDuplicateClone();
    }
    private void UnlockAttackClone()
    {
        if (cloneAttackUnlockButton.unLocked) { canAttack = true; attackMultiplier = cloneAttackMultiplier; }
    }
    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unLocked) { aggresiveCloneUnlocked = true;attackMultiplier = aggresiveCloneAttackMultiplier; }
    }
    private void UnlockDuplicateClone()
    {
        if (duplicateCloneUnlockButton.unLocked) { canDuplicateClone = true;attackMultiplier = multiCloneAttackMultiplier; }
    }
    private void UnlockCrystalClone()
    {
        if (crystalCloneUnlockButton.unLocked) { isCrystalClone = true; }
    }
    #endregion
    public void CreatClone(Transform _clonePosition, Vector3 _offset)
    {
        if (isCrystalClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab,_clonePosition.position,Quaternion.identity);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player,attackMultiplier);
        
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CreatCloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0))); 
    }
    private IEnumerator CreatCloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreatClone(_transform, _offset);
    }
}
