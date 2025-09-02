using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPool
{
    private Queue<GameObject> _queue = new Queue<GameObject>();
    private GameObject _prefab;

    public void Initialize(EnemyData data)
    {
        _prefab = data.Prefab;

        for (int i = 0; i < data.PoolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab);
            obj.SetActive(false);
            _queue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (_queue.Count > 0)
        {
            obj = _queue.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(_prefab);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _queue.Enqueue(obj);
    }
}