using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] Button acceptBtn;
    [SerializeField] Button rejectBtn;

    public void AcceptQuest()
    {
        Debug.Log("����Ʈ ����");
    }

    public void RejectQuest()
    {
        Debug.Log("����Ʈ ����");
    }
}
