using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSlot : InventorySlot
{
    protected Armor slotArmorItemData;

    public Armor SlotArmorItemData
    {
        get => slotArmorItemData;
        set
        {
            slotArmorItemData = value;
            slotItemData = value;
            DisplayItemImage();
        }
    }

    private void Awake()
    {
        slotType = eSlotType.Inventory;
        slotItemType = eItemType.Armor;
    }
}
