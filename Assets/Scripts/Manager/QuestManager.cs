using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Mesh;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;

    private Dictionary<int, QuestData> inProgressQuestDict;
    private Dictionary<int, QuestData> completedQuestDict;

    public static QuestManager Instance
    {
        get => instance;
    }

    public Dictionary<int, QuestData> InProgressQuestDict
    {
        get => inProgressQuestDict;
    }

    private void Awake()
    {
        instance = this;

        inProgressQuestDict = new Dictionary<int, QuestData>();
        completedQuestDict = new Dictionary<int, QuestData>();

        // 임시 할당
        RecieveInProgressQuest(DataTableManager.Instance.InitializeDataStruct<QuestData>(eDataTable.Quest, 1));
        RecieveInProgressQuest(DataTableManager.Instance.InitializeDataStruct<QuestData>(eDataTable.Quest, 2));
    }

    public void RecieveInProgressQuest(QuestData recivedQuest)
    {
        if (!inProgressQuestDict.ContainsKey(recivedQuest.id))
        {
            inProgressQuestDict.Add(recivedQuest.id, recivedQuest);
        }
    }

    public void ClearQuest(QuestData recivedQuest)
    {
        inProgressQuestDict.Remove(recivedQuest.id);
        completedQuestDict.Add(recivedQuest.id, recivedQuest);
    }
}
