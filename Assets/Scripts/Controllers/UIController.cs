using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : Singleton<UIController>
{
    public Inventory Inventory { get => inventory; }

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private DarkGauge darkGauge;
    [SerializeField]
    private TextMeshProUGUI dungeonText;
    [SerializeField]
    private TextMeshProUGUI floorText;
    [SerializeField]
    private GameObject damageTagCanvas;
    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    private Inventory inventory;

    private Player _player;

    public void ShowInventory()
    {
        if (CinematicController.Instance.IsPlaying)
            return;
        inventory.gameObject.SetActive(!inventory.IsOpen);
        inventory.IsOpen = !inventory.IsOpen;
    }

    public void HideInventory()
    {
        if (!inventory.IsOpen)
            return;
        inventory.gameObject.SetActive(false);
        inventory.IsOpen = false;
    }

    public void ShowShop()
    {
        shopPanel.SetActive(true);
        shopPanel.GetComponent<ShopPanel>().Open();
    }

    public void HideShop()
    {
        shopPanel.GetComponent<ShopPanel>().Close();
        shopPanel.SetActive(false);
    }

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

    public void ShowDamageTag(Vector3 position, int damage)
    {
        GameObject obj = Instantiate(damageTagCanvas, position, Quaternion.identity);
        DamageTag tag = obj.GetComponentInChildren<DamageTag>();
        tag.SetDamage(damage);
    }

    protected override void Awake()
    {
        base.Awake();
        inventory.gameObject.SetActive(false);
        
    }

    private void Start()
    {
        GameController.Instance.Player.Controls.UI.Inventory.performed += context => ShowInventory();
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
