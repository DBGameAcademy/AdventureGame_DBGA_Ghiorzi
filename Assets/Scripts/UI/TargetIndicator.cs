using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject arrowIndicator;
    [SerializeField]
    private GameObject squareIndicator;

    private bool _isActive = true;

    public void Active()
    {
        if (_isActive)
            return;
        _isActive = true;
        arrowIndicator.SetActive(true);
        squareIndicator.SetActive(true);
    }

    public void Deactive()
    {
        if(!_isActive)
            return ;
        _isActive = false;
        arrowIndicator.SetActive(false);
        squareIndicator.SetActive(false);
    }

    private void Awake()
    {
        Deactive();
    }
}
