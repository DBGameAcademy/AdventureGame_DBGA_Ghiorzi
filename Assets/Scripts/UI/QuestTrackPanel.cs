using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTrackPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questText;

    private void Start()
    {
        UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        string questTextString = "No Quest!";

        if (QuestManager.Instance.CurrentQuest)
        {
            questTextString = QuestManager.Instance.CurrentQuest.QuestName;
            questTextString += "\n";
            questTextString += QuestManager.Instance.CurrentQuest.QuestText;
            questTextString += "\n";
            questTextString += QuestManager.Instance.CurrentQuest.GetObjectiveText();
        }

        questText.text = questTextString;
    }
}
