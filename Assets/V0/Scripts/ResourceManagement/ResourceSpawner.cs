using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceEntry
{
    public ResourceData resourceData;
    public BoxCollider2D spawnArea;
    [HideInInspector] public ObjectPool objectPool;
}

public class ResourceSpawner : MonoBehaviour
{
    public List<ResourceEntry> resources;
    private List<GameObject> activeResources = new List<GameObject>();

    private void Awake()
    {
        InitializePools();
        SpawnAllResources();
    }

    private void InitializePools()
    {
        foreach (var entry in resources)
        {
            ObjectPool pool = new ObjectPool();
            pool.Initialize(entry.resourceData.Prefab, entry.resourceData.PoolSize);
            entry.objectPool = pool;
        }
    }

    public void RespawnAllResourcesForNewDay()
    {
        ReturnAllResources();
        SpawnAllResources();
    }

    private void SpawnAllResources()
    {
        foreach (var entry in resources)
        {
            for (int i = 0; i < entry.resourceData.PoolSize; i++)
            {
                SpawnResource(entry);
            }
        }
    }

    private void ReturnAllResources()
    {

        foreach (var obj in activeResources)
        {
            obj.SetActive(false);
        }
        activeResources.Clear();
    }

    private void SpawnResource(ResourceEntry entry)
    {
        GameObject obj = entry.objectPool.GetObject();

        Bounds bounds = entry.spawnArea.bounds;
        Vector3 pos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );

        obj.transform.position = pos;


        Resource resourceComponent = obj.GetComponent<Resource>();
        if (resourceComponent != null)
        {
            resourceComponent.InitializeResource(entry.resourceData);
        }
        else
        {
            Debug.LogError($"The resource prefab '{entry.resourceData.name}' is missing a Resource script component.");
        }

        activeResources.Add(obj);
    }
}