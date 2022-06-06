using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public Quest CurrentQuest { get; private set; }

    [SerializeField]
    private QuestCollection Quests;

    public bool IsComplete()
    {
        for(int i = 0; i< CurrentQuest.Objectives.Count; ++i)
        {
            if(!CurrentQuest.Objectives[i].IsComplete)
                return false;
        }

        return true;
    }

    public void AddQuestKill(Monster killed)
    {
        if(CurrentQuest != null && !CurrentQuest.IsComplete)
        {
            for(int i =0; i<CurrentQuest.Objectives.Count; ++i)
            {
                if(CurrentQuest.Objectives[i] is KillObjective)
                {
                    KillObjective killObj = (KillObjective)CurrentQuest.Objectives[i];
                    if(killObj.TargetType == killed.MonsterID)
                    {
                        CurrentQuest.Objectives[i].CompleteObjective();
                    }
                }
            }
        }
    }
}
