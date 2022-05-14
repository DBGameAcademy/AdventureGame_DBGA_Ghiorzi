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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetBool("IsMoving", player.IsMoving);

        if(this.transform.position.y > 0.28f)
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
}
