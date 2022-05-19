using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    private float _valueToChange;
    private float _speedChange = 2f;
    private bool _isChanging;

    public void SetUp(int maxValue, int currentValue)
    {
        if (currentValue > maxValue)
            return;

        _slider.maxValue = maxValue;
        _slider.value = currentValue;
    }

    public void SetValue(int value)
    {
        //_slider.value = value;
        _valueToChange = value;
        _isChanging = true;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (_isChanging)
        {
            if(_slider.value > _valueToChange)
            {
                _slider.value -= Time.deltaTime*_speedChange;
            }
            else
            {
                _isChanging = false;
            }
        }

    }
}
