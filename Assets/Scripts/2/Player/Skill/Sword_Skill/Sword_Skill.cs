using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private SkillTreeSlot_UI bounceSwordUnlockButton;
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private SkillTreeSlot_UI pierceSwordUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private SkillTreeSlot_UI spinSwordUnlockButton;
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    [SerializeField] private SkillTreeSlot_UI swordUnlockButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive skill")]
    [SerializeField] private SkillTreeSlot_UI timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlot_UI vulnerableUnlockButton;
    public bool vulnerableUnlocked { get;private set; }

    private Vector2 finaDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParet;
    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        GenerateDots();

        //swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordSkill);
        bounceSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSwordSkill);
        pierceSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSwordSkill);
        spinSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSwordSkill);
        //timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordTimeStopSkill);
        //vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordVolnurableSkill);
    }
    private void SetupGravity()
    {
        if (swordType == SwordType.Pierce) { swordGravity = pierceGravity; }
        else if (swordType == SwordType.Bounce) { swordGravity = bounceGravity; }
        else if (swordType == SwordType.Spin) { swordGravity = spinGravity; }
    }
    protected override void Update()
    {
        base.Update();
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        { 
            finaDir = new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y); }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
        SetupGravity();
        swordUnlocked = swordUnlockButton.unLocked;
        timeStopUnlocked = timeStopUnlockButton.unLocked;
        vulnerableUnlocked = vulnerableUnlockButton.unLocked;
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        SkillManager.instance.sword.CanUseSkill();
        if (swordType == SwordType.Bounce) { newSwordScript.SetupBounce(true, amountOfBounce, bounceSpeed); }
        else if (swordType == SwordType.Pierce) { newSwordScript.SetupPierce(pierceAmount); }
        else if (swordType == SwordType.Spin) { newSwordScript.SetupSipn(true, maxTravelDistance, spinDuration, hitCooldown); }

        newSwordScript.SetUpSword(finaDir, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }
    #region AimEvent

    private Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParet);
            dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
    #region ½âËø¼¼ÄÜ
    //protected override void CheckUnlock()
    //{
    //    UnlockSwordSkill();
    //    UnlockPierceSwordSkill();
    //    UnlockBounceSwordSkill();
    //    UnlockSpinSwordSkill();
    //    UnlockSwordTimeStopSkill();
    //    UnlockSwordVolnurableSkill();
    //}
    //private void UnlockSwordSkill()
    //{
    //    if (swordUnlockButton.unLocked)
    //    {
    //        swordType = SwordType.Regular;
    //        swordUnlocked = true;
    //    }
    //}
    private void UnlockBounceSwordSkill()
    {
        if (bounceSwordUnlockButton.unLocked) { swordType = SwordType.Bounce; }
    }
    private void UnlockPierceSwordSkill()
    {
        if (pierceSwordUnlockButton.unLocked) { swordType = SwordType.Pierce; }
    }
    private void UnlockSpinSwordSkill()
    {
        if (spinSwordUnlockButton.unLocked) { swordType = SwordType.Spin; }
    }
    //private void UnlockSwordTimeStopSkill()
    //{
    //    if (timeStopUnlockButton.unLocked) { timeStopUnlocked = true; }
    //}
    //private void UnlockSwordVolnurableSkill()
    //{
    //    if (vulnerableUnlockButton.unLocked) { vulnerableUnlocked = true; }
    //}
    #endregion
}
