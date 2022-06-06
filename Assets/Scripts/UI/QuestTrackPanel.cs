using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTrackPanel : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    [SerializeField]
    private TextMeshProUGUI questText;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateQuestText();
    }

    public void Open()
    {
        if (IsOpen)
            return;
        _animator.SetBool("IsOpen", true);
        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen)
            return;
        _animator.SetBool("IsOpen", false);
        IsOpen = false;
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
