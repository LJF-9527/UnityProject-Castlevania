using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot_UI : ItemSlot_UI
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name="×°±¸²å²Û - "+slotType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;
        Inventory.instance.UnEquipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        CleanUpSlot();
        ui.itemToolTip.HideToolTip();
    }
}
