using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    public float AttackCooldown = 1f;
    private float _cooldownTimer = 0f;

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
            state = "Chase";
    }

    protected override void HandleChaseState()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;

        if (Vector2.Distance(transform.position, player.position) < 1f && _cooldownTimer <= 0)
            state = "Attack";
    }

    protected override void HandleAttackState()
    {
        _cooldownTimer = AttackCooldown;
        state = "Chase";
    }

    protected override void Attack()
    {
        base.Attack();
    }

    public override void TakeDamage(int amount)
    {
        Die(); 
    }
}