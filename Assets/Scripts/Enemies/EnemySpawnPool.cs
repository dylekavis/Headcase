using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int spawnCount;
    [SerializeField] int numToActivate;

    [SerializeField] float spawnTime;

    public float ActiveCount;

    List<GameObject> spawnedEnemies = new();

    bool isSpawning;

    void Awake()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, transform);
            spawnedEnemy.SetActive(false);
            spawnedEnemies.Add(spawnedEnemy);

            BombSpiderController bombSpider = spawnedEnemy.GetComponent<BombSpiderController>();
            bombSpider.RegisterSpawnPool(this);
        }
    }

    void Update()
    {
        if (ActiveCount < numToActivate && !isSpawning)
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        isSpawning = true;

        while (ActiveCount < numToActivate)
        {
            yield return new WaitForSeconds(spawnTime);

            foreach (var spawn in spawnedEnemies)
            {
                if (ActiveCount >= numToActivate)
                {
                    isSpawning = false;
                    break;
                } 
                
                if (spawn.gameObject.activeInHierarchy) continue;
                
                spawn.transform.position = transform.position;
                spawn.SetActive(true);
                ActiveCount++;
            }
        }
    }
}
