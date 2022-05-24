using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageTag : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI damageText;

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
