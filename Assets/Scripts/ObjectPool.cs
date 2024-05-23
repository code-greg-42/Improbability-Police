using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    protected List<GameObject> pooledObjects;
    [SerializeField] protected GameObject objectToPool;
    [SerializeField] protected int amountToPool;

    [HideInInspector]
    public bool isPoolLoaded; // flag for other scripts to see when the pool has been fully loaded

    protected virtual void Awake()
    {
        InitializePool();
    }

    protected virtual void InitializePool()
    {
        // initialize object pool with inactive objects
        pooledObjects = new List<GameObject>();
        GameObject obj;
        for (int i = 0; i < amountToPool; i++)
        {
            obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        isPoolLoaded = true;
    }

    public GameObject GetPooledObject()
    {
        // loop through and return the first inactive object
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
