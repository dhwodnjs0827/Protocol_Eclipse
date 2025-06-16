using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    protected eSlotType slotType;
    protected eItemType slotItemType;
    protected int slotIndex;
    protected Item slotItemData;
    [SerializeField] private Image itemImage;
    [SerializeField] private RectTransform rectTr;

    public eSlotType SlotType
    {
        get => slotType;
    }

    public eItemType SlotItemType
    {
        get => slotItemType;
    }

    public int SlotIndex
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    public Item SlotItemData
    {
        get => slotItemData;
    }

    public Image ItemImage
    {
        get => itemImage;
    }

    public RectTransform RectTr
    {
        get => rectTr;
    }

    protected void DisplayItemImage()
    {
        if (slotItemData == null)
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
        }
        else
        {
            itemImage.color = Color.white;
            itemImage.sprite = slotItemData.ItemImage;
        }
    }
}
