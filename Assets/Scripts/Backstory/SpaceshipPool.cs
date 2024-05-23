using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPool : ObjectPool
{
    public static SpaceshipPool Instance;
    [SerializeField] private GameObject[] spaceshipPrefabs; // array of spaceships

    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    protected override void InitializePool()
    {
        // Initialize object pool with inactive spaceship objects
        pooledObjects = new List<GameObject>();
        GameObject obj;
        for (int i = 0; i < amountToPool; i++)
        {
            // Randomly select a spaceship prefab
            GameObject randomPrefab = spaceshipPrefabs[Random.Range(0, spaceshipPrefabs.Length)];
            obj = Instantiate(randomPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        isPoolLoaded = true; // Set the flag to true after the pool is initialized
    }
}
