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
        if (monster.IsInBattle && monster.AttackDirection.x <= -0.9f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (monster.IsInBattle && monster.AttackDirection.x >= 0.9f)
        {
            _spriteRenderer.flipX = false;
        }
        _animator.SetBool("IsMoving", monster.IsMoving);

    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Hurt()
    {
        _animator.SetTrigger("Hurt");
    }
}
