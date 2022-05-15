using UnityEngine;

public class Monster: Actor
{
    private Reward _killReward;
    private int _damage;

    public void SetupMonster(MonsterData monsterData)
    {
        currentHealth = monsterData.Health;
        maxHealth = monsterData.Health;
        _damage = monsterData.Damage;
    }

    // Maybe will inherite from Actor - let's see when coding movements
    public void SetPosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    protected override void OnKill()
    {

    }
}