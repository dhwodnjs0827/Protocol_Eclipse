using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using System.IO;
using System;
using System.Reflection;
using SimpleJSON;


// DataTableManager
// 역할: CSV 및 JSON 데이터를 로드하여 메모리에 저장하고, 구조체로 매핑.
// 활용: 게임 내 퀘스트, 아이템, NPC 데이터 관리.
public class DataTableManager : Singleton<DataTableManager>
{
    private string absolutePath;
    private Dictionary<eDataTable, Dictionary<int, List<string>>> csvDataTable;
    private Dictionary<eDataTable, Dictionary<int, JSONNode>> jsonDataTable;

    public DataTableManager()
    {
        absolutePath = Application.dataPath + "/DataTables/";
        csvDataTable = new Dictionary<eDataTable, Dictionary<int, List<string>>>();
        jsonDataTable = new Dictionary<eDataTable, Dictionary<int, JSONNode>>();
        InitializeCSVDataTable();
        InitializeJSONDataTable();
    }

    // CSV 데이터 테이블을 로드한 후 Dictionary에 저장
    private void InitializeCSVDataTable()
    {
        string relativePath = absolutePath + "CSV/";
        DirectoryInfo directoryInfo = new DirectoryInfo(relativePath);
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            string dataPath = string.Empty;
            if (file.Extension.Equals(".csv"))
            {
                dataPath = relativePath + file.Name;
                eDataTable tableType = (eDataTable)Enum.Parse(typeof(eDataTable), file.Name.Replace("DataTable.csv", ""));
                using (StreamReader sr = new StreamReader(dataPath))
                {
                    string header = sr.ReadLine();
                    string line = string.Empty;
                    Dictionary<int, List<string>> dict;
                    List<string> list;
                    while (sr.Peek() > 0)
                    {
                        line = sr.ReadLine();
                        line = line.Trim();
                        string[] arrData = line.Split(',');
                        int dataID = int.Parse(arrData[0]);
                        foreach (string data in arrData)
                        {
                            if (csvDataTable.TryGetValue(tableType, out dict))
                            {
                                if (dict.TryGetValue(dataID, out list))
                                {
                                    list.Add(data);
                                }
                                else
                                {
                                    list = new List<string>();
                                    list.Add(data);
                                    dict.Add(dataID, list);
                                }
                            }
                            else
                            {
                                dict = new Dictionary<int, List<string>>();
                                list = new List<string>();
                                list.Add(data);
                                dict.Add(dataID, list);
                                csvDataTable.Add(tableType, dict);
                            }
                        }
                    }
                }
            }
        }
    }

    // JSON 데이터 테이블을 로드한 후 Dictionary에 저장
    private void InitializeJSONDataTable()
    {
        string relativePath = absolutePath + "JSON/";
        DirectoryInfo directoryInfo = new DirectoryInfo(relativePath);
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            string dataPath = string.Empty;
            if (file.Extension.Equals(".json"))
            {
                dataPath = relativePath + file.Name;
                eDataTable tableType = (eDataTable)Enum.Parse(typeof(eDataTable), file.Name.Replace("DataTable.json", ""));
                string jsonString = File.ReadAllText(dataPath);
                JSONArray jsonArray = JSON.Parse(jsonString).AsArray;
                foreach (JSONNode data in jsonArray)
                {
                    int dataID = data["ID"];
                    if (!jsonDataTable.TryGetValue(tableType, out Dictionary<int, JSONNode> dict))
                    {
                        dict = new Dictionary<int, JSONNode>();
                        jsonDataTable.Add(tableType, dict);
                    }
                    if (!dict.ContainsKey(dataID))
                    {
                        dict[dataID] = data;
                    }
                }
            }
        }
    }

    // 데이터 테이블 종류에 따라 CSV 테이블일 경우 해당 구조체에 맵핑
    // JSON 테이블일 경우 테이블에 맞는 메서드를 구현해 해당 메서드 호출
    public T InitializeDataStruct<T>(eDataTable tableType, int dataID) where T : struct
    {
        if (tableType == eDataTable.Quest && typeof(T) == typeof(QuestData))
        {
            return (T)(object)QuestDataMapping(dataID);
        }
        else
        {
            int dataIndex = 0;
            T dataStruct = new T();
            Dictionary<int, List<string>> dict = csvDataTable[tableType];
            List<string> list = dict[dataID];
            foreach (var field in typeof(T).GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    var enumValue = Enum.Parse(field.FieldType, list[dataIndex]);
                    field.SetValueDirect(__makeref(dataStruct), enumValue);
                }
                else
                {
                    field.SetValueDirect(__makeref(dataStruct), Convert.ChangeType(list[dataIndex], field.FieldType));
                }
                dataIndex++;
            }
            return dataStruct;
        }
    }

    private QuestData QuestDataMapping(int questID)
    {
        JSONNode data = jsonDataTable[eDataTable.Quest][questID];
        QuestData questData = new QuestData();
        questData.id = data["ID"];
        questData.name = data["Name"];
        questData.description = data["Description"];
        // 오류 수정 필요
        //questData.objectives = ParseObjectives(data["Objectives"].AsArray);
        questData.rewards = ParseRewards(data["Rewards"]);
        questData.questType = (eQuestType)Enum.Parse(typeof(eQuestType), data["Quest Type"]);
        questData.questGiver = ParseGiver(data["Quest Giver"]);
        questData.requirements = ParseRequirements(data["Requirements"]);
        questData.dialogue = ParseDialogue(data["Dialogue"]);
        questData.status = (eQuestStatus)Enum.Parse(typeof(eQuestStatus), data["Status"]);
        return questData;
    }

    private QuestObjectives[] ParseObjectives(JSONArray jsonArray)
    {
        List<QuestObjectives> list = new List<QuestObjectives>();
        foreach (JSONObject data in jsonArray)
        {
            QuestObjectives objective = new QuestObjectives();
            objective.type = (eObjectivesType)Enum.Parse(typeof(eObjectivesType), data["Type"]);
            objective.targetType = (eDataTable)Enum.Parse(typeof(eDataTable), data["Target Type"]);
            objective.targetID = data["Target ID"];
            objective.targetName = data["Target Name"];
            objective.quantity = data["Quantity"];
            list.Add(objective);
        }
        return list.ToArray();
    }

    private QuestRewards ParseRewards(JSONNode jsonNode)
    {
        QuestRewards rewards = new QuestRewards();
        rewards.exp = jsonNode["exp"];
        rewards.gold = jsonNode["gold"];
        rewards.items = ParseItems(jsonNode["items"].AsArray);
        return rewards;
    }

    private QuestItems[] ParseItems(JSONArray jsonArray)
    {
        List<QuestItems> list = new List<QuestItems>();
        foreach (JSONObject data in jsonArray)
        {
            QuestItems item = new QuestItems();
            item.itemType = (eItemType)Enum.Parse(typeof(eItemType), data["Type"]);
            item.itemID = data["Item ID"];
            item.itemName = data["Item Name"];
            item.quantity = data["Quantity"];
        }
        return list.ToArray();
    }

    private QuestGiver? ParseGiver(JSONNode jsonNode)
    {
        if (jsonNode == null)
        {
            return null;
        }

        QuestGiver giver = new QuestGiver();
        giver.npcID = jsonNode["NPC ID"];
        giver.npcName = jsonNode["NPC Name"];
        return giver;
    }

    private QuestRequirements? ParseRequirements(JSONNode jsonNode)
    {
        if (jsonNode == null)
        {
            return null;
        }

        QuestRequirements requirement = new QuestRequirements();
        requirement.requiredQuestID = jsonNode["Required Quest ID"];
        requirement.requiredMinLevel = jsonNode["Required Min Level"];
        return requirement;
    }

    private QuestDialogue ParseDialogue(JSONNode jsonNode)
    {
        QuestDialogue dialogue = new QuestDialogue();
        dialogue.start = jsonNode["Start"];
        dialogue.inProgress = jsonNode["InProgress"];
        dialogue.complete = jsonNode["Complete"];
        return dialogue;
    }
}
