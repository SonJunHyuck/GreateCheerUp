using System.Collections;
using UnityEngine;

public abstract class UnitAttackTargetRanged : UnitAttack
{
    // Projectile
    protected Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    protected string projectileTag;

    protected override void Start()
    {
        base.Start();

        projectileSpawnPoint = transform.Find("EffectSpawnPoint");
    }

    public override void TryAttack(float direction)
    {
        base.TryAttack(direction);
    }

    // FireProjectile
    public override void Attack(float direction)
    {   
        InvokeStateChangedEvent(true);  // 애니메이션 실행
        
        float dist = Mathf.Abs(transform.position.x - CurrentTarget.position.x);
        StartCoroutine(DelaiedFire(direction, dist));
    }

    protected abstract IEnumerator DelaiedFire(float direction, float distance);
}
