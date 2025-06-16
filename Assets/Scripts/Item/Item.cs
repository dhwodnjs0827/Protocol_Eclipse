using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    protected ItemData itemData;
    protected Sprite itemImage;

    public int ItemID
    {
        get => itemData.id;
    }

    public Sprite ItemImage
    {
        get => itemImage;
    }

    public Item(int dataID)
    {
        itemData = DataTableManager.Instance.InitializeDataStruct<ItemData>(eDataTable.Item, dataID);
    }
}
