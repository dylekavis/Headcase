using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnPool : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int spawnCount;
    [SerializeField] int numToActivate;

    [SerializeField] float spawnTime;
    [SerializeField] float spawnRadius;

    public float ActiveCount;

    List<GameObject> spawnedObjects = new();

    public bool isSpawning;

    void Awake()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, transform);
            spawnedObject.SetActive(false);
            spawnedObjects.Add(spawnedObject);

            ThrowableObject throwObj = spawnedObject.GetComponent<ThrowableObject>();
            throwObj.RegisterSpawnPool(this);
        }
    }

    void Update()
    {
        if (ActiveCount < numToActivate && !isSpawning)
        {
            StartCoroutine(SpawnObjectsRoutine());
        }
    }

    IEnumerator SpawnObjectsRoutine()
    {
        isSpawning = true;

        while (ActiveCount < numToActivate)
        {
            yield return new WaitForSeconds(spawnTime);

            foreach (var spawn in spawnedObjects)
            {
                if (ActiveCount >= numToActivate)
                {
                    isSpawning = false;
                    break;
                }

                if (spawn.gameObject.activeInHierarchy) continue;
                
                Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
                spawn.transform.position = (Vector2)transform.position + randomPoint;
                spawn.SetActive(true);
                ActiveCount++;
            }
        }

        isSpawning = false;
    }
}
