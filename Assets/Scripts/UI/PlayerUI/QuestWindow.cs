using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] RectTransform content;
    private Rect contentRect;

    private Vector3 startPos;
    private Vector3 offset;

    private List<QuestInfoSlot> slotList;

    private void Awake()
    {
        contentRect = UIRectCalculate.RectInfo(content);

        startPos = new Vector3(0f, 155f, 0f);
        offset = new Vector3(0f, -155f, 0f);

        slotList = new List<QuestInfoSlot>();
    }

    private void Start()
    {
        // 임시 코드
        Dictionary<int, QuestData> tmpDict = QuestManager.Instance.InProgressQuestDict;
        CreateNewSlot(tmpDict[1].name, tmpDict[1].description);
        CreateNewSlot(tmpDict[2].name, tmpDict[2].description);
    }

    private void CreateNewSlot(string name, string objectives)
    {
        GameObject slotResouce = ResourceManager.Instance.GetPrefabResource(DataDefinition.ePrefabType.UI, "QuestInfoSlot");
        QuestInfoSlot newSlot = Instantiate(slotResouce).AddComponent<QuestInfoSlot>();
        newSlot.gameObject.transform.SetParent(content.gameObject.transform);
        newSlot.transform.localPosition = startPos;

        if (slotList.Count > 0)
        {
            newSlot.transform.localPosition = startPos + (offset * slotList.Count);
        }

        newSlot.SetQuestInfo(name, objectives);

        if (slotList.Count > 2)
        {
            newSlot.gameObject.SetActive(false);
        }
        slotList.Add(newSlot);
    }
}
