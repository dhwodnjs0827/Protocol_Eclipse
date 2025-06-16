using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedArmorSlot : ArmorSlot
{
    private eArmorType slotArmorType;

    public eArmorType SlotArmorType
    {
        get => slotArmorType;
        set
        {
            if (slotArmorType == eArmorType.None)
            {
                slotArmorType = value;
            }
        }
    }

    private void Awake()
    {
        slotType = eSlotType.EquippedArmor;
        slotItemType = eItemType.Armor;
    }
}
