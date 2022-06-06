using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public Quest CurrentOffer { get; set; }
    public bool IsOpen { get; private set; }

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI acceptCompleteButtonText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private TextMeshProUGUI goldRewardText;
    [SerializeField]
    private List<GameObject> rewardItemIcons = new List<GameObject>();

    private Animator _animator;

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

    private void Awake()
    {
        IsOpen = false;
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(CurrentOffer == null)
        {
            GetNewQuest();
        }
    }

    private void GetNewQuest()
    {
        if(CurrentOffer == null && QuestManager.Instance.CurrentQuest == null)
        {
            CurrentOffer = QuestManager.Instance.Quests.GetRandomQuest();
            CurrentOffer.ResetQuest();
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if(QuestManager.Instance.CurrentQuest != null)
        {
            titleText.text = QuestManager.Instance.CurrentQuest.QuestName;
            descriptionText.text = QuestManager.Instance.CurrentQuest.QuestText;
            if (QuestManager.Instance.CurrentQuest.IsComplete)
            {
                acceptCompleteButtonText.text = "COMPLETE";
            }
            else
            {
                acceptCompleteButtonText.text = "ABANDON";
            }
        }
        else
        {
            if(CurrentOffer == null)
            {
                GetNewQuest();
            }
            titleText.text = CurrentOffer.QuestName;
            descriptionText.text = CurrentOffer.QuestText;
            acceptCompleteButtonText.text = "ACCEPT";
        }
    }

    public void AcceptCompleteButtonClicked()
    {
        if(QuestManager.Instance.CurrentQuest != null)
        {
            if (QuestManager.Instance.CurrentQuest.IsComplete)
            {
                // TO-DO: Reward player
            }
            QuestManager.Instance.CurrentQuest = null;
            UpdateDisplay();
        }
        else
        {
            QuestManager.Instance.CurrentQuest = CurrentOffer;
            CurrentOffer = null;
            UpdateDisplay();
        }
    }
}
