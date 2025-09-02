using UnityEngine;

public class SelfDestructEnemy : BaseEnemy
{
    public int Damage = 20;
    public float ExplodeRange = 5f;
    public float DetectionTime = 3f;

    private float _timer = 0f;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        RunState();
    }

    protected override void HandlePatrolState()
    {
        if (Vector2.Distance(transform.position, player.position) < SeePlayerRange)
        {
            state = "Chase";
            _timer = 0f;
        }
    }

    protected override void HandleChaseState()
    {
        _timer += Time.deltaTime;
        if (_timer >= DetectionTime || Vector2.Distance(transform.position, player.position) < ExplodeRange)
            state = "Attack";
    }

    protected override void HandleAttackState()
    {
        
    }

    protected override void Attack()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= ExplodeRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(Damage);
            }
        }

        Die();
    }

    public override void TakeDamage(int amount)
    {
        Die(); 
    }
}