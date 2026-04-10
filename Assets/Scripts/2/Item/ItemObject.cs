using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        SetupVisual();
    }
    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;

        rb.velocity = _velocity;
        SetupVisual();
    }
    private void SetupVisual()
    {
        if (itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = itemData.itemType + " - " + itemData.equipmentType + " - " + itemData.itemName;
    }
    public void PickUpItem()
    {
        if(!Inventory.instance.CanAddItem()&&itemData.itemType==ItemType.Equipment) 
        {
            rb.velocity=new Vector2(3,7);
            PlayerManager.instance.player.fX.CreatePopUpText("¿â´æÒÑÂú", Color.white);
            return; 
        }
        Inventory.instance.AddItem(itemData);
        AudioManager.instance.PlaySFX(18, transform);
        Destroy(gameObject);
    }
}
