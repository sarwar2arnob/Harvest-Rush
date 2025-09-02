using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    private Queue<GameObject> queue = new Queue<GameObject>();
    private GameObject prefab;


    public void Initialize(GameObject prefabToPool, int size)
    {
        prefab = prefabToPool;
        for (int i = 0; i < size; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (queue.Count > 0)
        {
            GameObject obj = queue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        queue.Enqueue(obj);
    }
}