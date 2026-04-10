using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keycodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTime;

    private bool canGrow=true;
    private bool canShrink;
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    private bool playercanDisapear=true;

    private int amountOfAttack = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetUpBlackHole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttack, float _cloneAttackCooldown,float _blackholeTime)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCooldown= _cloneAttackCooldown;
        blackholeTime = _blackholeTime;
        if (SkillManager.instance.clone.isCrystalClone)
            playercanDisapear = false;
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTime -= Time.deltaTime;
        if (blackholeTime < 0)
        {
            blackholeTime = Mathf.Infinity;
            if (targets.Count > 0) { ReleasedBlackHoleAttack(); }
            else { FinishBlackHoleAbility(); }
        }
        if (Input.GetKeyDown(KeyCode.R))
        { ReleasedBlackHoleAttack(); }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0) { Destroy(gameObject); }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased&&amountOfAttack>0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            float xOffset;
            xOffset = (Random.Range(0, 100) > 50) ? 2 : -2;
            if(SkillManager.instance.clone.isCrystalClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
            SkillManager.instance.clone.CreatClone(targets[randomIndex], new Vector3(xOffset, 0));
            }
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackHoleAbility", .8f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
        DestroyHotKey();
        
    }

    private void ReleasedBlackHoleAttack()
    {
        if(targets.Count <=0) { return; }


        DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKey = false;
        if (playercanDisapear)
        {
            playercanDisapear = false;
            PlayerManager.instance.player.fX.MakeTransprent(true);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keycodeList.Count <= 0) { return; }
        if (!canCreateHotKey) { return; }
        GameObject newHotKey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keycodeList[Random.Range(0, keycodeList.Count)];
        keycodeList.Remove(choosenKey);

        BlackHole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<BlackHole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }
    private void DestroyHotKey()
    {
        if (createdHotKey.Count <= 0) { return; }
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
        createdHotKey.Clear();
    }
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
