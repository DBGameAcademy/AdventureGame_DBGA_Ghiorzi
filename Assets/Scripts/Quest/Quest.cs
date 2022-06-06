using UnityEngine;
using System.Collections.Generic;

public class Quest : ScriptableObject
{
    public string QuestName;
    public string QuestText;
    public List<QuestObjective> Objectives = new List<QuestObjective>();
    public Reward CompletionReward;
}