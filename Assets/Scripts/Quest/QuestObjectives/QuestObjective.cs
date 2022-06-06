using UnityEngine;
using System.Collections.Generic;

public abstract class QuestObjective : ScriptableObject
{
    public bool IsComplete { get; protected set; }
    public int ObjectiveCount { get; protected set; }

    public abstract bool CompleteObjective();
    public abstract string GetObjectiveText();

    public virtual void ResetObjective()
    {
        ObjectiveCount = 0;
        IsComplete = false;
    }



}