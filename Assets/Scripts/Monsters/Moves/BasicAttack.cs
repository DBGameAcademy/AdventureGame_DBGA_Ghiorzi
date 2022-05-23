using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Move
{
    public override string Name => "Basic Attack";

    private bool _isAttacking = false;
    private float _attackStartTime = 0;
    private float _attackDuration = 0.5f;

    public override void Process()
    {
        _attackStartTime = Time.time;
        _isAttacking = true;
        _monster.gameObject.GetComponentInChildren<MonsterAnimation>().Attack();
    }

    private void Update()
    {
        if (_isAttacking)
        {
            float t = (Time.time - _attackStartTime) / _attackDuration;
            Vector3 attackPos = new Vector3(_monster.Target.transform.position.x, 0.0f, _monster.Target.transform.position.z);
            Vector3 dir = attackPos - _monster.transform.position;
            _monster.AttackDirection = dir;
            float prevY = _monster.transform.position.y;
            _monster.transform.position = DungeonController.Instance.GetTile(_monster.CurrentPosition).TileObj.transform.position + dir * Mathf.PingPong(t, 0.5f);
            _monster.transform.position = new Vector3(_monster.transform.position.x, prevY, _monster.transform.position.z);
            if (t > 1f)
            {
                // Finish
                // procees damages and stuff
                _monster.Target.Damage(_monster.GetDamage());
                _isAttacking = false;
                GameController.Instance.EndTurn();
                Destroy(this);
            }
        }
    }
}
