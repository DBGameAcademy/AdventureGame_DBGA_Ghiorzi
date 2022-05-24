using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _didFallPlay = false;
    private PlayerFall _playerFall;

    private int _attackIndex = 0; // 3 attacks combo

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerFall = GetComponentInParent<PlayerFall>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.IsMoving)
        {
            if(player.MovingDirection == Vector2Int.left)
            {
                _spriteRenderer.flipX = true;
            }
            else if(player.MovingDirection == Vector2Int.right)
            {
                _spriteRenderer.flipX = false;
            }
        }
        if (player.IsAttacking && player.IsInBattle && player.AttackDirection.x <= -0.9f)
        {
            // Left
            _spriteRenderer.flipX = true;
        }
        else if (player.IsAttacking && player.IsInBattle && player.AttackDirection.x >= 0.9f)
        {
            // Right
            _spriteRenderer.flipX = false;
        }
        _animator.SetBool("IsMoving", player.IsMoving);

        if(_playerFall.IsFalling)
        {
            CinematicController.Instance.StartCinematic();
            _animator.SetBool("IsGrounded",false);
            _didFallPlay = true;
        }
        else if(_didFallPlay)
        {
            CinematicController.Instance.EndCinematic();
            _animator.SetBool("IsGrounded",true);
            _didFallPlay = false;
        }
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger("Attack"+(_attackIndex+1));
        _attackIndex = (_attackIndex + 1) % 3; // Circular attack 0->1->2->0
    }

    public void PlayerTransfrom()
    {
        _animator.SetTrigger("Transform");
        CinematicController.Instance.ZoomIn();
    }
}
