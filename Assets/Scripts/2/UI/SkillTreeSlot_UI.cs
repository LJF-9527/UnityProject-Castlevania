using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    private Image skillImage;
    [SerializeField] private GameObject skillInGameImage;

    [SerializeField] public int skillPrice;

    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unLocked;
    [SerializeField] private SkillTreeSlot_UI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot_UI[] shouldBeLocked;

    private void OnValidate()
    {
        gameObject.name = "¼¼ÄÜ²Û - " + skillName;
    }
    private void Update()
    {
        if (unLocked)
        {
            skillImage.color = Color.white;
        }
        else
            skillImage.color = lockedSkillColor;
        if (skillInGameImage != null)
        {
            skillInGameImage.SetActive(unLocked);
        }
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());

    }
    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        if (unLocked) { skillImage.color = Color.white; }
        
    }
    public void UnlockSkillSlot()
    {
        if (unLocked) { return; }

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unLocked == false)
            {
                return;
            }
        }
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unLocked == true)
            {
                return;
            }
        }

        if (PlayerManager.instance.HaveEnoughMoney(skillPrice) == false) { return; }
        unLocked = true;
        skillImage.color = Color.white;
        AudioManager.instance.PlaySFX(7);
        if(skillInGameImage != null)
        {
            skillInGameImage.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillPrice.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
    #region DataBase
    public void LoadData(GameData _data)
    {
        if (_data.skillTrees.TryGetValue(skillName, out bool value))
        {
            unLocked = value;
            
        }
    }

    public void SaveData(ref GameData _data)
    {

        if (_data.skillTrees.TryGetValue(skillName, out bool value))
        {
            _data.skillTrees.Remove(skillName);
            _data.skillTrees.Add(skillName, unLocked);
        }
        else
        {
            _data.skillTrees.Add(skillName, unLocked);
        }
        
    }
    #endregion
}
