using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "$"+GameController.Instance.Player.Money.ToString("0000");
    }
}
