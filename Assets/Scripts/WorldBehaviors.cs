using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehaviors : MonoBehaviour
{
    [SerializeField] AgentController agentController;
    [SerializeField] MonsterController monsterController;
    [SerializeField] public List<MonsterController> spawnedMonsterList = new List<MonsterController>();
    [SerializeField] Transform environmentLocation;
    [SerializeField] int monsterCount;
    [SerializeField] int timeBetweenSpawn = 1;
    [SerializeField] Transform spawnZone1, spawnZone2, spawnZone3, spawnZone4;

    public void SpawnAgent()
    {
        agentController.transform.localPosition = new Vector3 (0f, 1.1f, 0f);
    }

    public void CallSpawnMonster()
    {
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        Vector3 spawnZone1Pos = spawnZone1.transform.position;
        Vector3 spawnZone2Pos = spawnZone2.transform.position;
        Vector3 spawnZone3Pos = spawnZone3.transform.position;
        Vector3 spawnZone4Pos = spawnZone4.transform.position;

        if (spawnedMonsterList.Count != 0)
        {
            RemoveMonster(spawnedMonsterList);
        }

        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 randomSpawnZone1Pos = spawnZone1Pos + new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-4f, 4f));
            Vector3 randomSpawnZone2Pos = spawnZone2Pos + new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-4f, 4f));
            Vector3 randomSpawnZone3Pos = spawnZone3Pos + new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-4f, 4f));
            Vector3 randomSpawnZone4Pos = spawnZone4Pos + new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-4f, 4f));
            Vector3[] randomArea = { spawnZone1Pos, spawnZone2Pos, spawnZone3Pos, spawnZone4Pos };

            int counter = 0;
            bool distanceGood;

            MonsterController monster = Instantiate(monsterController);
            monster.target = agentController.transform;
            monster.transform.parent = environmentLocation;
            monster.worldBehaviors = this;
            Vector3 monsterLocation = randomArea[Random.Range(0, randomArea.Length)];

            if (spawnedMonsterList.Count == 0)
            {
                monster.MoveMonster(monsterLocation);
                spawnedMonsterList.Add(monster);
                yield return new WaitForSeconds(timeBetweenSpawn);
            } 
            else if (spawnedMonsterList.Count != 0)
            {
                for (int j = 0; j < spawnedMonsterList.Count; j++)
                {
                    if (counter < 10)
                    {
                        distanceGood = CheckOverLap(monsterLocation, spawnedMonsterList[j].transform.localPosition, 1f);
                        if (!distanceGood)
                        {
                            monsterLocation = randomArea[Random.Range(0, randomArea.Length)];
                            j--;
                        }
                        counter++;
                    }
                    else
                    {
                        j = spawnedMonsterList.Count;
                    }
                }
                monster.MoveMonster(monsterLocation);
                spawnedMonsterList.Add(monster);
                yield return new WaitForSeconds(timeBetweenSpawn);
            }
        }
    }

    public bool CheckOverLap(Vector3 objectToAvoidOverlap, Vector3 objectAlreadyExisted, float maxDistance)
    {
        float distance = Vector3.Distance(objectToAvoidOverlap, objectAlreadyExisted);
        if (maxDistance <= distance)
        {
            return true;
        }
        return false;
    }

    public int GetMonsterCount()
    {
        return spawnedMonsterList.Count;
    }

    private void RemoveMonster(List<MonsterController> spawnedMonsterList)
    {
        foreach (MonsterController monster in spawnedMonsterList)
        {
            Destroy(monster.gameObject);
        }
        spawnedMonsterList.Clear();
    }
}
