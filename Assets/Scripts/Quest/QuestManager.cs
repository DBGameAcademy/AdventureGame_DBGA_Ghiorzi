using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public Quest CurrentQuest { get; private set; }

    [SerializeField]
    private QuestCollection Quests;
}
