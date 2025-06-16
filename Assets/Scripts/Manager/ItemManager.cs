using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using System;

public class ItemManager : MonoBehaviour, CurrentEquippedWeaponObserver
{
    private static ItemManager instance;

    List<EquippedWeaponSlotObserver> equippedWeaponSlotObserverList;

    private Weapon[] equippedWeaponList;
    private Armor[] equippedArmorList;

    private Weapon[] inventoryWeapon;
    private Armor[] inventoryArmor;
    private ETC[] inventoryETC;

    private int emptySlotIndex;

    public static ItemManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        instance = this;

        equippedWeaponSlotObserverList = new List<EquippedWeaponSlotObserver>();

        equippedWeaponList = new Weapon[3];
        equippedArmorList = new Armor[3];

        inventoryWeapon = new Weapon[Constants.MaxItemListCount];
        inventoryArmor = new Armor[Constants.MaxItemListCount];
        inventoryETC = new ETC[Constants.MaxItemListCount];

        emptySlotIndex = -1;

        // 임시 할당
        // 무기 임시 할당
        AddItem(eItemType.Weapon, 311100);
        AddItem(eItemType.Weapon, 312100);
        AddItem(eItemType.Weapon, 313100);
        AddItem(eItemType.Weapon, 314100);
        // 방어구 임시 할당
        AddItem(eItemType.Armor, 321100);
        AddItem(eItemType.Armor, 322100);
        AddItem(eItemType.Armor, 323100);
        // 총알 임시 할당
        AddItem(eItemType.ETC, 331100, 400);
        AddItem(eItemType.ETC, 331200, 10);
        AddItem(eItemType.ETC, 331300, 400);
        AddItem(eItemType.ETC, 331400, 30);
        AddItem(eItemType.ETC, 331400, 70);
    }

    private void Start()
    {
        PlayManager.instance.Player.AddCurrentEquippedWeaponObserver(this);
    }

    public void AddItem(eItemType itemType, int itemID, int itemStack = 1)
    {
        switch (itemType)
        {
            case eItemType.Weapon:
                if (IsInventoryFull(inventoryWeapon))
                {
                    Debug.Log("무기 인벤토리 꽉 참");
                }
                else
                {
                    Weapon weapon = new Weapon(itemID);
                    inventoryWeapon[emptySlotIndex] = weapon;
                    return;
                }
                break;
            case eItemType.Armor:
                if (IsInventoryFull(inventoryArmor))
                {
                    Debug.Log("방어구 인벤토리 꽉 참");
                }
                else
                {
                    Armor armor = new Armor(itemID);
                    inventoryArmor[emptySlotIndex] = armor;
                    return;
                }
                break;
            case eItemType.ETC:
                int remainingStack = itemStack;
                for (int i = 0; i < inventoryETC.Length && remainingStack > 0; i++)
                {
                    if (inventoryETC[i] != null && inventoryETC[i].ItemID == itemID)
                    {
                        int overStack = inventoryETC[i].GetOverStack(remainingStack);
                        inventoryETC[i].IncreaseStack(remainingStack);
                        remainingStack = overStack;
                    }
                }
                for (int i = 0; i < inventoryETC.Length && remainingStack > 0; i++)
                {
                    if (inventoryETC[i] == null)
                    {
                        ETC newItem = new ETC(itemID);
                        int overStack = newItem.GetOverStack(remainingStack);
                        newItem.IncreaseStack(remainingStack);

                        inventoryETC[i] = newItem;
                        remainingStack = overStack;
                    }
                }
                if (remainingStack > 0)
                {
                    Debug.Log("방어구 인벤토리 꽉 참");
                }
                break;
            case eItemType.Module:
                break;
        }
    }

    public void UseETCItem(int itemID, int itemStack)
    {
        int remainingStack = itemStack;
        for (int i = inventoryETC.Length - 1; i >= 0 && remainingStack > 0; i--)
        {
            if (inventoryETC[i] != null && inventoryETC[i].ItemID.Equals(itemID))
            {
                remainingStack = inventoryETC[i].UseItem(remainingStack);
                if (inventoryETC[i].CurStack == 0)
                {
                    inventoryETC[i] = null;
                }
            }
        }
        if (remainingStack > 0)
        {
            Debug.Log("아이템 없음");
        }
    }

    private bool IsInventoryFull<T>(T[] inventoryItem) where T : Item
    {
        for (int i = 0; i < inventoryItem.Length; i++)
        {
            if (inventoryItem[i] == null)
            {
                emptySlotIndex = i;
                return false;
            }
        }
        emptySlotIndex = -1;
        return true;
    }

    public Weapon[] GetEquippedWeaponList()
    {
        return equippedWeaponList;
    }

    public Armor[] GetEquippedArmorList()
    {
        return equippedArmorList;
    }

    public T[] GetInventoryItemList<T>() where T : Item
    {

        if (typeof(T) == typeof(Weapon))
        {
            return inventoryWeapon as T[];
        }
        else if (typeof(T) == typeof(Armor))
        {
            return inventoryArmor as T[];
        }
        else if (typeof(T) == typeof(ETC))
        {
            return inventoryETC as T[];
        }
        return null;
    }

    public void SwapData(eInventoryState curState, eSlotType selectSlotType, int selectIndex, eSlotType targetSlotType, int targetIndex)
    {
        Item[] inventoryList = null;
        switch (curState)
        {
            case eInventoryState.Weapon:
                inventoryList = inventoryWeapon;
                break;
            case eInventoryState.Armor:
                inventoryList = inventoryArmor;
                break;
            case eInventoryState.ETC:
                inventoryList = inventoryETC;
                break;
        }

        Item[] selectItem = null;
        switch (selectSlotType)
        {
            case eSlotType.Inventory:
                selectItem = inventoryList;
                break;
            case eSlotType.EquippedWeapon:
                selectItem = equippedWeaponList;
                break;
            case eSlotType.EquippedArmor:
                selectItem = equippedArmorList;
                break;
        }

        Item[] targetItem = null;
        switch (targetSlotType)
        {
            case eSlotType.Inventory:
                targetItem = inventoryList;
                break;
            case eSlotType.EquippedWeapon:
                targetItem = equippedWeaponList;
                break;
            case eSlotType.EquippedArmor:
                targetItem = equippedArmorList;
                break;
        }

        Item tmp = selectItem[selectIndex];
        selectItem[selectIndex] = targetItem[targetIndex];
        targetItem[targetIndex] = tmp;

        if (selectSlotType == eSlotType.EquippedWeapon || targetSlotType == eSlotType.EquippedWeapon)
        {
            NotifyChangeEquippedWeapon(equippedWeaponList);
        }
        else if (selectSlotType == eSlotType.EquippedArmor || targetSlotType == eSlotType.EquippedArmor)
        {
            //player.UpdateArmorData(equippedArmorList);
        }
    }

    public int GetTotalBullet(eWeaponType weaponType)
    {
        int itemID = 0;
        switch (weaponType)
        {
            case eWeaponType.AR:
                itemID = 331100;
                break;
            case eWeaponType.SR:
                itemID = 331200;
                break;
            case eWeaponType.SMG:
                itemID = 331300;
                break;
            case eWeaponType.SG:
                itemID = 331400;
                break;
        }
        int total = 0;
        for (int i = 0; i < inventoryETC.Length; i++)
        {
            if (inventoryETC[i] != null && inventoryETC[i].ItemID.Equals(itemID))
            {
                total += inventoryETC[i].CurStack;
            }
        }
        return total;
    }

    public void AddObserver(EquippedWeaponSlotObserver observer)
    {
        if (!equippedWeaponSlotObserverList.Contains(observer))
        {
            equippedWeaponSlotObserverList.Add(observer);
        }
    }

    public void NotifyChangeEquippedWeapon(Weapon[] equippedWeapon)
    {
        foreach (var observer in equippedWeaponSlotObserverList)
        {
            observer.ChangeEquippedWeaponList(equippedWeapon);
        }
    }

    public void UseBullet(int equippedWeaponIndex)
    {
        equippedWeaponList[equippedWeaponIndex].ShotCurMagazine();
        switch (equippedWeaponList[equippedWeaponIndex].WeaponData.weaponType)
        {
            case eWeaponType.AR:
                UseETCItem(Constants.AR_BULLET_ID, 1);
                break;
            case eWeaponType.SR:
                UseETCItem(Constants.SR_BULLET_ID, 1);
                break;
            case eWeaponType.SMG:
                UseETCItem(Constants.SMG_BULLET_ID, 1);
                break;
            case eWeaponType.SG:
                UseETCItem(Constants.SG_BULLET_ID, 1);
                break;
        }
    }

    public void ReloadWeapon(int equippedWeaponIndex)
    {
        equippedWeaponList[equippedWeaponIndex].ReloadMagazine();
    }

    public void SwapCurWeapon(int equippedWeaponIndex)
    {
        // TODO: Implement this method
    }
}
