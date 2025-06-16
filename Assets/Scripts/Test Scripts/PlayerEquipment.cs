using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Weapon[] equippedWeaponList;
    private int curWeaponIndex;

    private Transform weaponObjectPool;
    private Transform playerWeaponPos;
    [SerializeField] private GameObject curWeaponObject;
    private Transform curWeaponMuzzlePos;

    public Weapon CurWeapon
    {
        get => equippedWeaponList[curWeaponIndex];
    }
    public Transform CurWeaponMuzzlePos
    {
        get => curWeaponMuzzlePos;
    }

    private void Awake()
    {
        equippedWeaponList = new Weapon[3];
        curWeaponIndex = 0;

        playerWeaponPos = gameObject.transform.Find("root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/WeaponPos");
    }

    private void Start()
    {
        Weapon firstWeapon = new Weapon(311100);
        equippedWeaponList[0] = firstWeapon;

        curWeaponMuzzlePos = curWeaponObject.transform.Find("MuzzlePos");
    }

    private void SetCurWeaponObject()
    {
        if (curWeaponObject != null)
        {
            curWeaponObject.transform.parent = weaponObjectPool;
            curWeaponObject.SetActive(false);
        }
        if (equippedWeaponList[curWeaponIndex] != null)
        {
            curWeaponObject = ObjectPoolManager.Instance.GetWeaponObject(equippedWeaponList[curWeaponIndex].ItemID);
            curWeaponObject.transform.parent = playerWeaponPos;
            curWeaponObject.transform.position = playerWeaponPos.position;
            curWeaponObject.transform.rotation = playerWeaponPos.rotation;

            curWeaponMuzzlePos = curWeaponObject.transform.Find("MuzzlePos");
        }
    }

    public void ChangeEquippedWeaponList(Weapon[] weaponList)
    {
        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i] != null)
            {
                equippedWeaponList[i] = weaponList[i];
            }
            else
            {
                equippedWeaponList[i] = null;
            }
        }

        SetCurWeaponObject();
    }
}
