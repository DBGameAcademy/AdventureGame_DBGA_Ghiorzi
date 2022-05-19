using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DarkGauge : MonoBehaviour
{
    [SerializeField]
    private GameObject flame;

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

    private void Update()
    {

        if (_slider.value == 0)
            flame.SetActive(false);
        if (_slider.value == _slider.maxValue)
            flame.SetActive(true);
    }
}
