using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;

public class EquippedWeaponSlot : WeaponSlot
{
    private void Awake()
    {
        slotType = eSlotType.EquippedWeapon;
        slotItemType = eItemType.Weapon;
    }
}
