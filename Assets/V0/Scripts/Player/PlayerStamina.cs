using UnityEngine;

public class PlayerStamina : PlayerStat
{
    [Header("Stamina Settings")]
    public float MoveDrain = 10f;
    public float AttackCost = 20f;
    public float RegenRate = 5f;

    [HideInInspector]
    public bool CanMove = true;

    private Vector2 moveInput;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        bool isMoving = moveInput != Vector2.zero;

        if (GetValue() <= 0)
        {
            CanMove = false;
            Regenerate();
        }
        else if (isMoving)
        {
            CanMove = true;
            Change(-MoveDrain * Time.deltaTime);
        }
        else
        {
            CanMove = true;
            Regenerate();
        }
    }
    public void Attack()
    {
        if (GetValue() <= 0)
        {
            Debug.Log("Too tired to attack!");
            return;
        }

        Change(-AttackCost);
        Debug.Log("Attack! Stamina left: " + GetValue());
    }

    private void Regenerate()
    {
        Change(RegenRate * Time.deltaTime);
    }
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }
}
