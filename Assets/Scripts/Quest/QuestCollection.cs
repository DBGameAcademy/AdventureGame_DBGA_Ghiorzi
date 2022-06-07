using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Collection", menuName = "Quest/Create New Quest Collection")]
public class QuestCollection : ScriptableObject
{
    public List<Quest> Quests = new List<Quest>();

    private int _index = 0;

    public Quest GetNextQuest()
    {
        Quest q =  Quests[_index];
        _index = (_index+1) % Quests.Count;
        return q;
    }
}
