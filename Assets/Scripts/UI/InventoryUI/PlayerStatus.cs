using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private string statusText;
    [SerializeField] private TextMeshProUGUI playerStatusTxt;

    private void Awake()
    {
        statusText = string.Empty;
        playerStatusTxt.text = statusText;
    }

    public void SetPlayerStatus()
    {
        statusText = string.Empty;
        PlayerStatusData playerData = PlayManager.instance.Player.PlayerStatus;
        Type dataStructType = typeof(PlayerStatusData);
        FieldInfo[] fields = dataStructType.GetFields();
        for (int i = 0; i < 8; i++)
        {
            if (i == 7)
            {
                statusText += fields[i].Name + ": " + fields[i].GetValue(playerData);
            }
            else
            {
                statusText += fields[i].Name + ": " + fields[i].GetValue(playerData) + '\n';
            }
        }
        playerStatusTxt.text = statusText;
    }
}
