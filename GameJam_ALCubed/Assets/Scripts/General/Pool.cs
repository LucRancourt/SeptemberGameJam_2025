using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool
{
    // Variables
    private List<GameObject> _pool = new List<GameObject>();
    private GameObject _poolObjectPrefab;


    // Constructor
    public Pool(int initialPoolSize, GameObject poolObjectPrefab)
    {
        if (!poolObjectPrefab)
        {
            Debug.LogError("Invalid Object for Pooling!");
            return;
        }

        _poolObjectPrefab = poolObjectPrefab;


        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewPoolObject();
        }
    }


    // Functions
    public GameObject PullFromPool()
    {
        if (!AnyAvailable())
            CreateNewPoolObject();

        return _pool.First(poolObject => !poolObject.activeInHierarchy);
    }

    private bool AnyAvailable()
    {
        return _pool.Any(poolObject => !poolObject.activeInHierarchy);
    }

    private void CreateNewPoolObject()
    {
        GameObject poolObject = Object.Instantiate(_poolObjectPrefab);
        poolObject.SetActive(false);
        _pool.Add(poolObject);
    }

    public void ReturnToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }

    public void ReturnAllToPool()
    {
        foreach (GameObject poolObject in _pool)
        {
            poolObject.SetActive(false);
        }
    }
}