using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CurrentEquippedWeaponObserver
{
    public void UseBullet(int equippedWeaponIndex);

    public void ReloadWeapon(int equippedWeaponIndex);

    public void SwapCurWeapon(int equippedWeaponIndex);
}
