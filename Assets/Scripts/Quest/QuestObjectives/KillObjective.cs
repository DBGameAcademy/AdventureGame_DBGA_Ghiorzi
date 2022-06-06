using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjective : QuestObjective
{
    public int TargetKillCount { get; set; }
    public eMonsterID TargetType { get; set; }

    public override bool CompleteObjective()
    {
        ObjectiveCount++;

        if(TargetKillCount > 0 && ObjectiveCount >= TargetKillCount)
        {
            IsComplete = true;
            return true;
        }

        return false;
    }

    public override string GetObjectiveText()
    {
        return "Kill " + TargetType.ToString() + " : " + ObjectiveCount + " / " + TargetKillCount;
    }
}
