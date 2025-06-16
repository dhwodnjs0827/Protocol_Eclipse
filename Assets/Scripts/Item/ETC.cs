using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETC : Item
{
    private ETCData etcData;
    private int curStack;
    private readonly int maxStack;

    public ETCData ETCData
    {
        get => etcData;
    }

    public int CurStack
    {
        get => curStack;
    }

    public ETC(int dataID) : base(dataID)
    {
        itemImage = ResourceManager.Instance.GetSpriteResource(eSpriteType.ETCItem, itemData.spriteResourceName);
        etcData = DataTableManager.Instance.InitializeDataStruct<ETCData>(eDataTable.ETC, dataID);
        maxStack = itemData.maxStack;
        curStack = 0;
    }

    public void IncreaseStack(int stack)
    {
        curStack = Math.Min(curStack + stack, maxStack);
    }

    public int UseItem(int stack)
    {
        int usedStack = Math.Min(curStack, stack);
        curStack -= usedStack;
        return stack - usedStack;
    }

    public int GetOverStack(int stack)
    {
        int total = curStack + stack;
        return total > maxStack ? total - maxStack : 0;
    }
}