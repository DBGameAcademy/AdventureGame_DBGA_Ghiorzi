using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Create New Quest")]
public class Quest : ScriptableObject
{
    public string QuestName;
    public string QuestText;
    public List<QuestObjective> Objectives = new List<QuestObjective>();
    public Reward CompletionReward;
    public bool IsComplete
    {
        get
        {
            foreach(QuestObjective objective in Objectives)
            {
                if (!objective.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void ResetQuest()
    {
        foreach(QuestObjective o in Objectives)
        {
            o.ResetObjective();
        }
    }

    public string GetObjectiveText()
    {
        string objectiveText = "";

        foreach(QuestObjective objective in Objectives)
        {
            objectiveText += objective.GetObjectiveText();
            objectiveText += "\n";
        }

        return objectiveText;
    }
}