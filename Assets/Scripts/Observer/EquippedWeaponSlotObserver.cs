using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EquippedWeaponSlotObserver
{
    public void ChangeEquippedWeaponList(Weapon[] equippedWeapon);
}
