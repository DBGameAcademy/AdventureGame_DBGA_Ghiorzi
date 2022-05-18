using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Animator))]
public class MonsterAnimation : MonoBehaviour
{
    [SerializeField]
    private Monster monster;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>(); 
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (monster.IsMoving)
        {
            if (monster.MovingDirection == Vector2Int.left)
            {
                _spriteRenderer.flipX = true;
            }
            else if (monster.MovingDirection == Vector2Int.right)
            {
                _spriteRenderer.flipX = false;
            }
        }
        if (monster.IsInBattle && monster.AttackDirection.x == -1)
        {
            // Left
            _spriteRenderer.flipX = false;
        }
        else if (monster.IsInBattle && monster.AttackDirection.x == 1)
        {
            // Right
            _spriteRenderer.flipX = true;
        }
        _animator.SetBool("IsMoving", monster.IsMoving);

    }

}
