using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed=2.0f;

    private Animator _animator;

    private bool _isMoving = false;
    private Vector3 _targetPosMove;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        TileSpawner.OnStopMoving += MoveMenu;
    }

    private void OnDisable()
    {
        TileSpawner.OnStopMoving -= MoveMenu;
    }

    private void Update()
    {
        if(_isMoving)
        {
            if(Mathf.Abs(transform.position.x - _targetPosMove.x) > 0.05)
            {
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            }
            else
            {
                _isMoving = false;
            }
        }
    }

    private void MoveMenu()
    {
        MenuTile tile = MenuTilePool.Instance.LastObject;

        MoveToPosition(tile.transform.position);
    }

    private void MoveToPosition(Vector3 targetPos)
    {
        if (_isMoving)
            return;
        _isMoving = true;
        _targetPosMove = targetPos;
    }
}
