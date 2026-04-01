using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int spawnCount;
    [SerializeField] int numToActivate;

    [SerializeField] float spawnTime;
    [SerializeField] float cameraDistance;

    public float ActiveCount;

    List<GameObject> spawnedEnemies = new();

    public bool isSpawning;

    public bool spawnOffScreen;

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

        if (Vector2.Distance(Camera.main.transform.position, transform.position) < cameraDistance && spawnOffScreen)
        {
            gameObject.SetActive(false);
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
                    yield break;
                } 
                
                if (spawn.gameObject.activeInHierarchy) continue;
                
                spawn.transform.position = transform.position;
                spawn.SetActive(true);
                ActiveCount++;
            }
        }

        isSpawning = false;
    }
}
