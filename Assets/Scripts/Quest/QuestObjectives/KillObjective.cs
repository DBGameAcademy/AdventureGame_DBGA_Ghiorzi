using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kill Objective", menuName = "Quest/Objectives/Create Kill Objective")]
public class KillObjective : QuestObjective
{
    public int TargetKillCount;
    public eMonsterID TargetType;

    public override bool CompleteObjective()
    {
        ObjectiveCount++;

        if(TargetKillCount > 0 && ObjectiveCount >= TargetKillCount)
        {
            IsComplete = true;
            UIController.Instance.UpdateQuestText();
            return true;
        }
        UIController.Instance.UpdateQuestText();
        return false;
    }

    public override string GetObjectiveText()
    {
        return "Kill " + TargetType.ToString() + " : " + ObjectiveCount + " / " + TargetKillCount;
    }
}
