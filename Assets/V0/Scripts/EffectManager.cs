using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private Dictionary<ParticleSystem, ObjectPool> effectPools = new Dictionary<ParticleSystem, ObjectPool>();
    private int defaultPoolSize = 10;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayEffect(ParticleSystem effectPrefab, Vector3 position)
    {
        PlayEffect(effectPrefab, position, Quaternion.identity);
    }

    public void PlayEffect(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
    {
        if (effectPrefab == null) return;

        if (!effectPools.ContainsKey(effectPrefab))
        {
            ObjectPool newPool = new ObjectPool();
            newPool.Initialize(effectPrefab.gameObject, defaultPoolSize);
            effectPools.Add(effectPrefab, newPool);
        }

        ObjectPool pool = effectPools[effectPrefab];
        GameObject effectObject = pool.GetObject();

        effectObject.transform.position = position;
        effectObject.transform.rotation = rotation;

        StartCoroutine(ReturnEffectToPool(effectObject, effectPrefab.main.duration, pool));
    }

    private IEnumerator ReturnEffectToPool(GameObject obj, float delay, ObjectPool pool)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnObject(obj);
    }
}