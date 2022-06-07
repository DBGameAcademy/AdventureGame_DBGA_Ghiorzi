using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TileSpawner : MonoBehaviour
{
    public static event Action OnStopMoving;

    [SerializeField]
    private float tileSpawnRate = 2.0f;

    private float _spawnTimer = 0.0f;
    private bool _stop = false;

    public void StopTiles()
    {
        _stop = true;
        OnStopMoving?.Invoke();
    }

    private void Update()
    {
        if(_stop)
            return;
        _spawnTimer += Time.deltaTime;
        if(_spawnTimer > tileSpawnRate)
        {
            _spawnTimer = 0.0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        var menuTile = MenuTilePool.Instance.Get();
        menuTile.transform.position = transform.position;
        menuTile.gameObject.SetActive(true);
    }
}
