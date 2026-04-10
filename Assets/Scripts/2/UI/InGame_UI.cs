using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] public Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    private Player2 player;
    private SkillManager skills;
    private void Awake()
    {
        
    }
    void Start()
    {
        player=PlayerManager.instance.player;
        if (playerStats != null) { playerStats.onHealthChange += UpdateHealthUI; }
        skills = SkillManager.instance;
        dashImage.transform.parent.gameObject.SetActive(skills.dash.dashUnlocked);
        parryImage.transform.parent.gameObject.SetActive(skills.parry.parryUnlocked);
        crystalImage.transform.parent.gameObject.SetActive(skills.crystal.canCreatCrystal);
        swordImage.transform.parent.gameObject.SetActive(skills.sword.swordUnlocked);
        blackholeImage.transform.parent.gameObject.SetActive(skills.blackHole.blackHoleUnlocked);
        UpdateHealthUI();
        

    }
    void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked) SetCooldownOf(dashImage);
        CheckCooldownOf(dashImage, skills.dash.cooldown);

        if (Input.GetKeyUp(KeyCode.Q) && skills.parry.parryUnlocked&&player.counterAttackState.isCounter) SetCooldownOf(parryImage);
        CheckCooldownOf(parryImage, skills.parry.cooldown);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.canCreatCrystal) SetCooldownOf(crystalImage);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);


        CheckCooldownOf(swordImage, skills.sword.cooldown);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackHole.blackHoleUnlocked) SetCooldownOf(blackholeImage);
        CheckCooldownOf(blackholeImage, skills.blackHole.cooldown);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipmentType(EquipmentType.Flask) != null) SetCooldownOf(flaskImage);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.CurrentCurrencyAmount())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.CurrentCurrencyAmount();
        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
    public void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount<=0) { _image.fillAmount = 1; }
    }
    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (player.IsWallDetected() && !player.IsGroundDetected()&&_image==dashImage) { _image.fillAmount = 0;return; }
        if (_image.fillAmount>0) { _image.fillAmount -=1/_cooldown*Time.deltaTime; }
    }
}


