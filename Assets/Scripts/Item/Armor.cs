using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    ArmorData armorData;

    public ArmorData GetArmorData()
    {
        return armorData;
    }

    public Armor(int dataID) : base(dataID)
    {
        armorData = DataTableManager.Instance.InitializeDataStruct<ArmorData>(eDataTable.Armor, dataID);
        itemImage = ResourceManager.Instance.GetSpriteResource(eSpriteType.Armor, itemData.spriteResourceName);
    }
}
