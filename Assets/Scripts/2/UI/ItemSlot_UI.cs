using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot_UI : MonoBehaviour,IPointerDownHandler ,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newItem)
    {
        CleanUpSlot();
        item=_newItem;
        itemImage.color= Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = string.Empty;
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite= null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item==null) return;
        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            
            return;
        }
        if(item.data.itemType==ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;
        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }
}
