using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTile : MonoBehaviour
{
    private float _moveSpeed = 2.0f;
    private float _currentLifeTime = 0.0f;
    private float _maxLifeTime = 10.0f;

    private bool _stop = false;

    private void OnEnable()
    {
        TileSpawner.OnStopMoving += Stop;
    }

    private void OnDisable()
    {
        TileSpawner.OnStopMoving -= Stop;
    }

    void Update()
    {
        if(_stop)
            return;
        transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        _currentLifeTime += Time.deltaTime;
        if (_currentLifeTime > _maxLifeTime)
        {
            MenuTilePool.Instance.ReturnToPool(this);
            _currentLifeTime = 0.0f;
        }
    }

    private void Stop()
    {
        _stop = true;
    }
}
