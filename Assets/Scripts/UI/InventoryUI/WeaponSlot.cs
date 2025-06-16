using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : InventorySlot
{
    protected Weapon slotWeaponItemData;

    public Weapon SlotWeaponItemData
    {
        get => slotWeaponItemData;
        set
        {
            slotWeaponItemData = value;
            slotItemData = value;
            DisplayItemImage();
        }
    }

    private void Awake()
    {
        slotType = eSlotType.Inventory;
        slotItemType = eItemType.Weapon;
    }
}
