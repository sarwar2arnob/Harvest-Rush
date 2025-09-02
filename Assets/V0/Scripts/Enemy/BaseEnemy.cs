using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float Speed = 2f;
    public float SeePlayerRange = 10f;
    public EnemyData enemyData;
    public float LifeTime = 180f;

    protected Transform player;
    protected Vector2 patrolTarget;
    [HideInInspector] public BoxCollider2D SpawnArea;

    protected string state = "Patrol";
    private bool _isDead = false;

    private void OnEnable()
    {
        Initialize();
        if (LifeTime > 0)
        {
            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null)
                spawner.StartCoroutine(spawner.RespawnEnemyAfterLifetime(this, LifeTime));
        }
    }

    protected void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        _isDead = false;
        state = "Patrol";
        PickNewPatrolPoint();
    }

    public void Respawn(Vector3 spawnPosition, BoxCollider2D spawnArea)
    {
        _isDead = false;
        transform.position = spawnPosition;
        SpawnArea = spawnArea;
        gameObject.SetActive(true);
        PickNewPatrolPoint();
    }

    protected void RunState()
    {
        if (_isDead || player == null) return;

        bool playerInArea = SpawnArea != null && SpawnArea.bounds.Contains(player.position);

        switch (state)
        {
            case "Patrol":
                Patrol();
                HandlePatrolState();
                break;
            case "Chase":
                if (playerInArea)
                {
                    Chase();
                    HandleChaseState();
                }
                else
                {
                    state = "Patrol";
                    PickNewPatrolPoint();
                }
                break;
            case "Attack":
                if (playerInArea)
                {
                    Attack();
                    HandleAttackState();
                }
                else
                {
                    state = "Patrol";
                    PickNewPatrolPoint();
                }
                break;
        }
    }

    protected void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, Speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
            PickNewPatrolPoint();
    }

    protected void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
        FaceTarget(player.position);
    }

    protected void PickNewPatrolPoint(float range = 3f)
    {
        Vector2 newPoint = (Vector2)transform.position + new Vector2(Random.Range(-range, range), Random.Range(-range, range));

        if (SpawnArea != null)
        {
            Bounds bounds = SpawnArea.bounds;
            newPoint.x = Mathf.Clamp(newPoint.x, bounds.min.x, bounds.max.x);
            newPoint.y = Mathf.Clamp(newPoint.y, bounds.min.y, bounds.max.y);
        }

        patrolTarget = newPoint;
    }

    protected virtual void Attack()
    {
        if (player == null) return;

        FaceTarget(player.position);
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Wall"))
            PickNewPatrolPoint();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Sword")) return;

        if (enemyData != null && enemyData.BloodEffect != null)
        {
            ParticleSystem blood = Instantiate(enemyData.BloodEffect, transform.position, Quaternion.identity);
            blood.Play();
            Destroy(blood.gameObject, 1f);
        }

        TakeDamage(1);
    }

    public virtual void Die()
    {
        if (_isDead) return;

        _isDead = true;
        gameObject.SetActive(false);

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            spawner.StartCoroutine(spawner.RespawnEnemyAfterDelay(this, 5f));
    }

    protected void FaceTarget(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        if (direction.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        else if (direction.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    protected abstract void HandlePatrolState();
    protected abstract void HandleChaseState();
    protected abstract void HandleAttackState();
    public abstract void TakeDamage(int amount);
}

