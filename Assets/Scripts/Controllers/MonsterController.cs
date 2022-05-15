using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : Singleton<MonsterController>
{
    public List<Monster> ActiveMonster { get=>_activeMonsters;}

    [SerializeField]
    private MonsterCollection monsterCollection;

    private List<Monster> _activeMonsters = new List<Monster>();

    public Monster AddMonster(eMonsterID monsterID)
    {
        Monster newMonster = monsterCollection.GetMonster(monsterID);
        ActiveMonster.Add(newMonster);
        return newMonster;
    }

    public void DestroyAllMonster()
    {
        foreach(Monster monster in ActiveMonster)
        {
            Destroy(monster.gameObject);
        }
        _activeMonsters.Clear();
    }
}
