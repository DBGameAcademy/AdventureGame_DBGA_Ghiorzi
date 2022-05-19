using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    public void SetUp(int maxValue, int currentValue)
    {
        if (currentValue > maxValue)
            return;

        _slider.maxValue = maxValue;
        _slider.value = currentValue;
    }

    public void SetValue(int value)
    {
        _slider.value = value;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
}
