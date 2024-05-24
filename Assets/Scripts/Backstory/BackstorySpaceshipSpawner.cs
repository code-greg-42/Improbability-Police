using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstorySpaceshipSpawner : MonoBehaviour
{
    // Array of X positions
    private readonly float[] xPositions = { -900f, -700f, -500f, -300f, -100f, 100f, 300f, 500f, 700f, 900f };

    // timing for a new ship block
    private readonly float shipSpawnDelay = 3.0f;

    private Coroutine spaceshipSpawnCoroutine;

    private bool hasSpawned = false; // flag for updating that ships have spawned

    void Update()
    {
        if (!hasSpawned && spaceshipSpawnCoroutine == null && BackstorySpaceshipPool.Instance.isPoolLoaded)
        {
            hasSpawned = true;
            spaceshipSpawnCoroutine = StartCoroutine(StartSpaceshipSpawn());
        }
    }

    private void SpawnSpaceshipBlock()
    {
        for (int i = 0; i < xPositions.Length; i++)
        {
            // Get a spaceship from the pool
            GameObject spaceship = BackstorySpaceshipPool.Instance.GetPooledObject();

            if (spaceship != null)
            {
                // Set the position of the spaceship
                spaceship.transform.position = new Vector3(xPositions[i], 0f, 0f);
                // Activate the spaceship
                spaceship.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No pooled spaceship available");
            }
        }
    }

    private IEnumerator StartSpaceshipSpawn()
    {
        while (true)
        {
            SpawnSpaceshipBlock();
            yield return new WaitForSeconds(shipSpawnDelay);
        }
    }

    public void StopSpawn()
    {
        if (spaceshipSpawnCoroutine != null)
        {
            StopCoroutine(spaceshipSpawnCoroutine);
        }
    }
}
