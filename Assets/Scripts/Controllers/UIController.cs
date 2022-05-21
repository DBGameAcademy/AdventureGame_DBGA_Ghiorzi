using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : Singleton<UIController>
{
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private DarkGauge darkGauge;
    [SerializeField]
    private TextMeshProUGUI dungeonText;
    [SerializeField]
    private TextMeshProUGUI floorText;
    

    private Player _player;

    public void ShowBattleUI()
    {
        healthBar.gameObject.SetActive(true);
        darkGauge.gameObject.SetActive(true);
    }

    public void HideBattleUI()
    {
        healthBar.gameObject.SetActive(false);
        darkGauge.gameObject.SetActive(false);
    }

    public void ShowDungeonInfo(int floorIndex)
    {
        floorText.gameObject.SetActive(true);
        dungeonText.gameObject.SetActive(true);
        floorIndex++;
        floorText.text = "Floor -" + floorIndex;
    }

    public void HideDungeonInfo()
    {
        floorText.gameObject.SetActive(false);
        dungeonText.gameObject.SetActive(false);
    }

    public void SetUpPlayer(Player player)
    {
        if(player == null)
            return;
        _player = player;
        healthBar.SetUp(_player.MaxHealth, _player.CurrentHealth);
        healthBar.gameObject.SetActive(false);
        darkGauge.SetUp(_player.MaxDarkValue, _player.DarkValue);
        darkGauge.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
        if (healthBar.gameObject.activeSelf)
        {
            healthBar.SetValue(_player.CurrentHealth);
        }
        if (darkGauge.gameObject.activeSelf)
        {
            darkGauge.SetValue(_player.DarkValue);
        }
    }

}
