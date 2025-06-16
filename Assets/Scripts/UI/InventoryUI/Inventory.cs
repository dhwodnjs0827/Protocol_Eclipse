using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
    [Space]
    [SerializeField] private PlayerStatus playerStatus;
    [Space]
    [SerializeField] private EquippedWeaponSlot[] equippedWeaponSlotList;
    [SerializeField] private EquippedArmorSlot[] equippedArmorSlotList;
    private WeaponSlot[] weaponSlotList;
    private ArmorSlot[] armorSlotList;
    private ETCSlot[] etcSlotList;
    [Space]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Image viewport;
    [SerializeField] private GameObject weaponContent;
    [SerializeField] private GameObject armorContent;
    [SerializeField] private GameObject etcContent;
    [Space]
    [SerializeField] private Button weaponBtn;
    [SerializeField] private Button armorBtn;
    [SerializeField] private Button etcBtn;
    [Space]
    [SerializeField] private Image moveSlot;
    [SerializeField] private TextMeshProUGUI moveSlotStackText;
    [Space]
    [SerializeField] private ItemInfoWindow itemInfoWindow;
    bool isDragging;

    InventorySlot selectSlot;
    InventorySlot targetSlot;
    InventorySlot curMosueSlot;

    private eInventoryState curInventory;

    private void Awake()
    {
        weaponSlotList = weaponContent.gameObject.GetComponentsInChildren<WeaponSlot>();
        armorSlotList = armorContent.gameObject.GetComponentsInChildren<ArmorSlot>();
        etcSlotList = etcContent.gameObject.GetComponentsInChildren<ETCSlot>();

        moveSlotStackText.text = string.Empty;

        isDragging = false;

        curInventory = eInventoryState.Weapon;
    }

    private void Start()
    {
        playerStatus.SetPlayerStatus();

        for (int i = 0; i < 3; i++)
        {
            equippedWeaponSlotList[i].SlotIndex = i;
            equippedArmorSlotList[i].SlotIndex = i;
        }
        for (int i = 0; i < 30; i++)
        {
            weaponSlotList[i].SlotIndex = i;
            armorSlotList[i].SlotIndex = i;
            etcSlotList[i].SlotIndex = i;
        }

        equippedArmorSlotList[0].SlotArmorType = eArmorType.Head;
        equippedArmorSlotList[1].SlotArmorType = eArmorType.Body;
        equippedArmorSlotList[2].SlotArmorType = eArmorType.Leg;
    }

    private void OnDisable()
    {
        selectSlot = null;
        targetSlot = null;
        curMosueSlot = null;
        moveSlot.gameObject.SetActive(false);

        isDragging = false;

        curInventory = eInventoryState.Weapon;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
        selectSlot = GetMouseSlot(mousePos);
        if (selectSlot != null && selectSlot.SlotItemData != null)
        {
            selectSlot.ItemImage.enabled = false;
            if (selectSlot.SlotItemType == eItemType.ETC)
            {
                ((ETCSlot)selectSlot).ItemStackText.enabled = false;
                moveSlotStackText.text = ((ETCSlot)selectSlot).ItemStackText.text;
                moveSlotStackText.enabled = true;
            }
            moveSlot.sprite = selectSlot.ItemImage.sprite;
            moveSlot.transform.position = mousePos;
            moveSlot.gameObject.SetActive(true);
            scrollRect.enabled = false;
            isDragging = true;

            if (itemInfoWindow.gameObject.activeSelf)
            {
                curMosueSlot = null;
                itemInfoWindow.SetItemInfo(null);
                itemInfoWindow.gameObject.SetActive(false);
                viewport.raycastTarget = true;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 mousePos = eventData.position;
            moveSlot.transform.position = mousePos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
        targetSlot = GetMouseSlot(mousePos);
        if (moveSlot.gameObject.activeSelf)
        {
            if (targetSlot != null)
            {
                SwapItem(selectSlot, targetSlot);
            }
            selectSlot.ItemImage.enabled = true;
            if (selectSlot.SlotItemType == eItemType.ETC)
            {
                ((ETCSlot)selectSlot).ItemStackText.enabled = true;
                moveSlotStackText.text = string.Empty;
                moveSlotStackText.enabled = false;
            }
            moveSlot.sprite = null;
            moveSlot.gameObject.SetActive(false);
            scrollRect.enabled = true;
            selectSlot = null;
            targetSlot = null;
            isDragging = false;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!isDragging)
        {
            Vector2 mousePos = eventData.position;
            curMosueSlot = GetMouseSlot(mousePos);
            if (curMosueSlot != null && curMosueSlot.SlotItemData != null)
            {
                itemInfoWindow.SetItemInfo(curMosueSlot);
                itemInfoWindow.transform.position = mousePos;
                itemInfoWindow.gameObject.SetActive(true);
                viewport.raycastTarget = false;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        curMosueSlot = null;
        itemInfoWindow.SetItemInfo(null);
        itemInfoWindow.gameObject.SetActive(false);
        viewport.raycastTarget = true;
    }

    public void SetEquipmentSlot()
    {
        Weapon[] weapon = ItemManager.Instance.GetEquippedWeaponList();
        for (int i = 0; i < equippedWeaponSlotList.Length; i++)
        {
            equippedWeaponSlotList[i].SlotWeaponItemData = weapon[i];
        }
        Armor[] armor = ItemManager.Instance.GetEquippedArmorList();
        for (int i = 0; i < equippedArmorSlotList.Length; i++)
        {
            equippedArmorSlotList[i].SlotArmorItemData = armor[i];
        }
    }

    public void SetWeaponList()
    {
        Weapon[] list = ItemManager.Instance.GetInventoryItemList<Weapon>();
        if (list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                weaponSlotList[i].SlotWeaponItemData = list[i];
            }
        }

        curInventory = eInventoryState.Weapon;

        weaponContent.gameObject.SetActive(true);
        armorContent.gameObject.SetActive(false);
        etcContent.gameObject.SetActive(false);

        weaponBtn.interactable = false;
        armorBtn.interactable = true;
        etcBtn.interactable = true;
    }

    void SetArmorList()
    {
        Armor[] list = ItemManager.Instance.GetInventoryItemList<Armor>();
        if (list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                armorSlotList[i].SlotArmorItemData = list[i];
            }
        }

        curInventory = eInventoryState.Armor;

        weaponContent.gameObject.SetActive(false);
        armorContent.gameObject.SetActive(true);
        etcContent.gameObject.SetActive(false);

        weaponBtn.interactable = true;
        armorBtn.interactable = false;
        etcBtn.interactable = true;
    }

    void SetETCList()
    {
        ETC[] list = ItemManager.Instance.GetInventoryItemList<ETC>();
        if (list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                etcSlotList[i].SlotETCItemData = list[i];
            }
        }

        curInventory = eInventoryState.ETC;

        weaponContent.gameObject.SetActive(false);
        armorContent.gameObject.SetActive(false);
        etcContent.gameObject.SetActive(true);

        weaponBtn.interactable = true;
        armorBtn.interactable = true;
        etcBtn.interactable = false;
    }

    InventorySlot GetMouseSlot(Vector2 mousePos)
    {
        for (int i = 0; i < equippedWeaponSlotList.Length; i++)
        {
            Rect rect = UIRectCalculate.RectInfo(equippedWeaponSlotList[i].RectTr);
            if (UIRectCalculate.IsInRect(rect, mousePos))
            {
                return equippedWeaponSlotList[i];
            }
        }
        for (int i = 0; i < equippedArmorSlotList.Length; i++)
        {
            Rect rect = UIRectCalculate.RectInfo(equippedArmorSlotList[i].RectTr);
            if (UIRectCalculate.IsInRect(rect, mousePos))
            {
                return equippedArmorSlotList[i];
            }
        }
        switch (curInventory)
        {
            case eInventoryState.Weapon:
                for (int i = 0; i < weaponSlotList.Length; i++)
                {
                    Rect rect = UIRectCalculate.RectInfo(weaponSlotList[i].RectTr);
                    if (UIRectCalculate.IsInRect(rect, mousePos))
                    {
                        return weaponSlotList[i];
                    }
                }
                break;
            case eInventoryState.Armor:
                for (int i = 0; i < armorSlotList.Length; i++)
                {
                    Rect rect = UIRectCalculate.RectInfo(armorSlotList[i].RectTr);
                    if (UIRectCalculate.IsInRect(rect, mousePos))
                    {
                        return armorSlotList[i];
                    }
                }
                break;
            case eInventoryState.ETC:
                for (int i = 0; i < etcSlotList.Length; i++)
                {
                    Rect rect = UIRectCalculate.RectInfo(etcSlotList[i].RectTr);
                    if (UIRectCalculate.IsInRect(rect, mousePos))
                    {
                        return etcSlotList[i];
                    }
                }
                break;
        }
        return null;
    }

    void SwapItem(InventorySlot selectSlot, InventorySlot targetSlot)
    {
        if (IsSwap(selectSlot, targetSlot))
        {
            ItemManager.Instance.SwapData(curInventory, selectSlot.SlotType, selectSlot.SlotIndex, targetSlot.SlotType, targetSlot.SlotIndex);

            if (selectSlot.SlotType == eSlotType.EquippedWeapon || targetSlot.SlotType == eSlotType.EquippedWeapon)
            {
                playerStatus.SetPlayerStatus();
            }
            else if (selectSlot.SlotType == eSlotType.EquippedArmor || targetSlot.SlotType == eSlotType.EquippedArmor)
            {
                playerStatus.SetPlayerStatus();
            }

            SetEquipmentSlot();
            switch (curInventory)
            {
                case eInventoryState.Weapon:
                    SetWeaponList();
                    break;
                case eInventoryState.Armor:
                    SetArmorList();
                    break;
                case eInventoryState.ETC:
                    SetETCList();
                    break;
            }
        }
    }

    bool IsSwap(InventorySlot selectSlot, InventorySlot targetSlot)
    {
        switch (selectSlot.SlotType)
        {
            case eSlotType.Inventory:
                switch (selectSlot.SlotItemType)
                {
                    case eItemType.Weapon:
                        if (targetSlot.SlotType == eSlotType.Inventory || targetSlot.SlotType == eSlotType.EquippedWeapon)
                        {
                            return true;
                        }
                        break;
                    case eItemType.Armor:
                        if (targetSlot.SlotType == eSlotType.Inventory)
                        {
                            return true;
                        }
                        else if (targetSlot.SlotType == eSlotType.EquippedArmor)
                        {
                            if (((ArmorSlot)selectSlot).SlotArmorItemData.GetArmorData().armorType == ((EquippedArmorSlot)targetSlot).SlotArmorType)
                            {
                                return true;
                            }
                        }
                        break;
                    case eItemType.ETC:
                        if (targetSlot.SlotType == eSlotType.Inventory)
                        {
                            return true;
                        }
                        break;
                }
                break;
            case eSlotType.EquippedWeapon:
                if (targetSlot.SlotType == eSlotType.EquippedWeapon)
                {
                    return true;
                }
                else if (curInventory == eInventoryState.Weapon && targetSlot.SlotType == eSlotType.Inventory)
                {
                    return true;
                }
                break;
            case eSlotType.EquippedArmor:
                if (curInventory == eInventoryState.Armor && targetSlot.SlotType == eSlotType.Inventory)
                {
                    if (targetSlot.SlotItemData == null)
                    {
                        return true;
                    }
                    if (((ArmorSlot)targetSlot).SlotArmorItemData.GetArmorData().armorType == ((ArmorSlot)selectSlot).SlotArmorItemData.GetArmorData().armorType)
                    {
                        return true;
                    }
                }
                break;
        }
        return false;
    }
}
