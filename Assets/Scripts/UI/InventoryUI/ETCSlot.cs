using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ETCSlot : InventorySlot
{
    private ETC slotETCItemData;
    [SerializeField] TextMeshProUGUI itemStackText;

    public ETC SlotETCItemData
    {
        get => slotETCItemData;
        set
        {
            slotETCItemData = value;
            slotItemData = value;
            DisplayItemImage();
            DisplayItemStack();
        }
    }

    public TextMeshProUGUI ItemStackText
    {
        get => itemStackText;
    }

    private void Awake()
    {
        slotType = eSlotType.Inventory;
        slotItemType = eItemType.ETC;
    }

    private void DisplayItemStack()
    {
        if (slotETCItemData != null)
        {
            int itemCurStack = slotETCItemData.CurStack;
            itemStackText.text = itemCurStack.ToString();
        }
        else
        {
            itemStackText.text = string.Empty;
        }
    }
}
