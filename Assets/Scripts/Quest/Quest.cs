using UnityEngine;
using System.Collections.Generic;

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
}