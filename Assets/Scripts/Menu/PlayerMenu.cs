using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private float moveSpeed=2.0f;

    private Animator _animator;

    private bool _isMoving = false;
    private bool _shouldJump = false;
    private bool _shouldGoDown = false;
    private bool _doOnce = false;
    private Vector3 _targetPosMove;
    private Vector3 _targetPosJump;

    public void Jump()
    {
        _shouldJump = true;
        _targetPosJump = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
    }

    public void GoDown()
    {
        _shouldGoDown = true;
        // Stop camera follow
        virtualCamera.m_Follow = null;
    }

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
                // Arrived
                _isMoving = false;
                _animator.SetTrigger("Jump");
            }
        }
        if (_shouldJump)
        {
            if (Mathf.Abs(transform.position.x - _targetPosJump.x) > 0.05)
            {
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            }
            else
            {
                // Jump Finished
                _shouldJump = false;
            }
        }
        if (_shouldGoDown)
        {
            // Go down undef
            transform.position += Vector3.down * Time.deltaTime * moveSpeed * 2f;
        }

        if (transform.position.y < -Camera.main.orthographicSize)
        {
            if (!_doOnce)
            {
                MenuManager.Instance.LoadScene("Game");
                _doOnce = true;
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
