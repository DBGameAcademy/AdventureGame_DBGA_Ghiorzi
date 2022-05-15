using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Collection", menuName = "Create Monster Collection")]
public class MonsterCollection : ScriptableObject
{
    public List<MonsterData> MonstersData = new List<MonsterData>();

    public Monster GetMonster(eMonsterID id)
    {
        MonsterData data = null;
        for(int i=0; i< MonstersData.Count; i++)
        {
            if(MonstersData[i].ID == id)
            {
                data = MonstersData[i];
                break;
            }
        }

        // Instantiate the monster
        GameObject newMonsterObj = Instantiate(data.Prefab, data.Prefab.transform.position, Quaternion.identity);
        Monster monster = newMonsterObj.GetComponent<Monster>();

        monster.SetupMonster(data);
        return monster;
    }

}
