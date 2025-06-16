using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWindow : MonoBehaviour
{
    private string infoText;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemInfoTxt;

    private void Awake()
    {
        infoText = string.Empty;
    }

    private void OnDisable()
    {
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        infoText = string.Empty;
        itemInfoTxt.text = infoText;
        gameObject.SetActive(false);
    }

    public void SetItemInfo(InventorySlot item)
    {
        if (item != null)
        {
            itemImage.sprite = item.ItemImage.sprite;
            itemImage.color = Color.white;
            SetInfoText(item);
            itemInfoTxt.text = infoText;
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            infoText = string.Empty;
            itemInfoTxt.text = infoText;
        }
    }

    private void SetInfoText(InventorySlot item)
    {
        infoText = string.Empty;
        switch (item.SlotItemType)
        {
            case eItemType.Weapon:
                WeaponData weaponData = ((WeaponSlot)item).SlotWeaponItemData.WeaponData;
                WeaponDataToString(weaponData);
                break;
            case eItemType.Armor:
                ArmorData armorData = ((ArmorSlot)item).SlotArmorItemData.GetArmorData();
                ArmorDataToString(armorData);
                break;
            case eItemType.ETC:
                ETCData etcData = ((ETCSlot)item).SlotETCItemData.ETCData;
                ETCDataToString(etcData);
                break;
        }
    }

    private void WeaponDataToString(WeaponData weaponData)
    {
        Type dataStructType = typeof(WeaponData);
        FieldInfo[] fields = dataStructType.GetFields();
        for (int i = 0; i < 8; i++)
        {
            if (i == fields.Length - 1)
            {
                infoText += fields[i].Name + ": " + fields[i].GetValue(weaponData);
            }
            else
            {
                infoText += fields[i].Name + ": " + fields[i].GetValue(weaponData) + '\n';
            }
        }
    }

    private void ArmorDataToString(ArmorData armorData)
    {
        Type dataStructType = typeof(ArmorData);
        FieldInfo[] fields = dataStructType.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            if (i == fields.Length - 1)
            {
                infoText += fields[i].Name + ": " + fields[i].GetValue(armorData);
            }
            else
            {
                infoText += fields[i].Name + ": " + fields[i].GetValue(armorData) + '\n';
            }
        }
    }

    private void ETCDataToString(ETCData etcData)
    {
        Type dataStructType = typeof(ETCData);
        FieldInfo[] fields = dataStructType.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            if (i == fields.Length - 1)
            {
                infoText += fields[i].Name + ": " + fields[i].GetValue(etcData);
            }
            else
            {
                infoText += fields[i].Name + ": " + fields[i].GetValue(etcData) + '\n';
            }
        }
    }
}
