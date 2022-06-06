using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Collection", menuName = "Quest/Create New Quest Collection")]
public class QuestCollection : ScriptableObject
{
    public List<Quest> Quests = new List<Quest>();

    public Quest GetRandomQuest()
    {
        return Quests[Random.Range(0, Quests.Count)];
    }
}