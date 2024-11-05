using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackRanged : UnitAttack
{
    // Projectile
    protected Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    protected string projectileTag;

    protected override void Start()
    {
        base.Start();

        projectileSpawnPoint = transform.Find("EffectSpawnPoint(Clone)");
    }

    public override void TryAttack(float direction)
    {
        base.TryAttack(direction);
    }

    // FireProjectile
    public override void Attack(float direction)
    {   
        InvokeStateChangedEvent(true);  // 애니메이션 실행

        StartCoroutine(DelaiedFire(direction));
    }

    protected abstract IEnumerator DelaiedFire(float direction);
}
