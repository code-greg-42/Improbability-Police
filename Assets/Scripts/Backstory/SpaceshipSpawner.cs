using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{
    // Array of X positions
    private readonly float[] xPositions = { -900f, -700f, -500f, -300f, -100f, 100f, 300f, 500f, 700f, 900f };
    private readonly float shipSpawnDelay = 4.2f;

    private Coroutine spaceshipSpawnCoroutine;

    void Update()
    {
        if (spaceshipSpawnCoroutine == null && SpaceshipPool.Instance.isPoolLoaded)
        {
            spaceshipSpawnCoroutine = StartCoroutine(StartSpaceshipSpawn());
        }
    }

    private void SpawnSpaceshipBlock()
    {
        for (int i = 0; i < xPositions.Length; i++)
        {
            // Get a spaceship from the pool
            GameObject spaceship = SpaceshipPool.Instance.GetPooledObject();

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
}
