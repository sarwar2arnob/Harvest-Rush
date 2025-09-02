using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyEntry
{
    public EnemyData EnemyData;
    public BoxCollider2D SpawnArea;
    [HideInInspector] public EnemyPool EnemyPool;
}

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyEntry> Enemies;

    private void Start()
    {
        InitializePools();
        SpawnAllEnemies();
    }

    private void InitializePools()
    {
        foreach (var entry in Enemies)
        {
            if (entry == null || entry.EnemyData == null) continue;

            EnemyPool pool = new EnemyPool();
            pool.Initialize(entry.EnemyData);
            entry.EnemyPool = pool;
        }
    }

    private void SpawnAllEnemies()
    {
        foreach (var entry in Enemies)
        {
            //if (entry == null || entry.EnemyData == null) continue;

            for (int i = 0; i < entry.EnemyData.PoolSize; i++)
                SpawnEnemy(entry);
        }
    }

    private void SpawnEnemy(EnemyEntry entry)
    {
        if (entry == null || entry.EnemyPool == null) return;

        GameObject enemy = entry.EnemyPool.GetObject();
        if (enemy == null) return;

        Vector3 spawnPos = GetRandomPoint(entry.SpawnArea);
        enemy.transform.position = spawnPos;

        BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
            baseEnemy.SpawnArea = entry.SpawnArea;
    }

    public IEnumerator RespawnEnemyAfterDelay(BaseEnemy enemy, float delay)
    {
        if (enemy == null) yield break;

        yield return new WaitForSeconds(delay);
        Vector3 spawnPos = GetRandomPoint(enemy.SpawnArea);
        enemy.Respawn(spawnPos, enemy.SpawnArea);
    }

    public IEnumerator RespawnEnemyAfterLifetime(BaseEnemy enemy, float lifetime)
    {
        if (enemy == null) yield break;

        yield return new WaitForSeconds(lifetime);
        enemy.Die(); 
    }

    private Vector3 GetRandomPoint(BoxCollider2D area)
    {
        if (area == null) return Vector3.zero;

        Bounds bounds = area.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }

    public void RespawnAllEnemies()
    {
        foreach (var entry in Enemies)
        {
            if (entry == null || entry.EnemyData == null) continue;

            for (int i = 0; i < entry.EnemyData.PoolSize; i++)
                SpawnEnemy(entry);
        }
    }
}
