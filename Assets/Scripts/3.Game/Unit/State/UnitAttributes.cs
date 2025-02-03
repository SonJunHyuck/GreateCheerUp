using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public string key;
    private float attack;

    public float Attack
    {
        get { return attack; }
        set
        {
            attack = value;
            if (TryGetComponent<UnitAttack>(out var unitAttack))
            {
                unitAttack.attackDamage = value;
            }
        }
    }
    private float speed;
    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            if(TryGetComponent<ActorMovement>(out var movement))
            {
                 movement.moveSpeed = value;
            }
        }
    }
    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            if(TryGetComponent<UnitHealth>(out var health))
            {
                health.maxHealth = value;
            }
        }
    }
}