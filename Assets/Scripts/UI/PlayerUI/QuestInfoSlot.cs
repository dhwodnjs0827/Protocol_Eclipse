using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestInfoSlot : MonoBehaviour
{
    TextMeshProUGUI questName;
    TextMeshProUGUI questObjectives;

    private void Awake()
    {
        questName = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questObjectives = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetQuestInfo(string name, string objectives)
    {
        questName.text = name;
        questObjectives.text = objectives;
    }
}
