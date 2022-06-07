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
    private List<Image> rewardItemIcons = new List<Image>();

    private Animator _animator;

    public void Open()
    {
        if (IsOpen)
            return;
        _animator.SetBool("IsOpen", true);
        IsOpen = true;
        UpdateDisplay();
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
            CurrentOffer = QuestManager.Instance.Quests.GetNextQuest();
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

            UpdateRewards(QuestManager.Instance.CurrentQuest.CompletionReward);
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

            UpdateRewards(CurrentOffer.CompletionReward);
        }
    }

    public void AcceptCompleteButtonClicked()
    {
        if(QuestManager.Instance.CurrentQuest != null)
        {
            if (QuestManager.Instance.CurrentQuest.IsComplete)
            {
                // Reward player
                GameController.Instance.Player.AddMoney(QuestManager.Instance.CurrentQuest.CompletionReward.GoldReward);
                if(QuestManager.Instance.CurrentQuest.CompletionReward.ItemRewards.Count > 0 
                    && QuestManager.Instance.CurrentQuest.CompletionReward.ItemRewards.Count <= UIController.Instance.Inventory.FreeSpace)
                {
                   
                    foreach(Item item in QuestManager.Instance.CurrentQuest.CompletionReward.ItemRewards)
                    {
                        UIController.Instance.Inventory.AddItemToInventory(item);
                    }
                }
            }
            QuestManager.Instance.CurrentQuest = null;
            UIController.Instance.UpdateQuestText();
            UpdateDisplay();
        }
        else
        {
            QuestManager.Instance.CurrentQuest = CurrentOffer;
            CurrentOffer = null;
            UIController.Instance.UpdateQuestText();
            UpdateDisplay();
        }
    }

    private void UpdateRewards(Reward reward)
    {
        for(int i =0;i<rewardItemIcons.Count; ++i)
        {
            rewardItemIcons[i].gameObject.SetActive(i < reward.ItemRewards.Count);
            if(i<reward.ItemRewards.Count)
                rewardItemIcons[i].sprite = reward.ItemRewards[i].Image;
        }

        goldRewardText.text = "$" + reward.GoldReward.ToString("0000");
    }
}
