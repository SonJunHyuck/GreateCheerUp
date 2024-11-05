using UnityEngine;
using System.Collections;

public class PlayerRangedAttack : PlayerAttack
{
    public float projectileDamage = 5f;
    public float projectileSpeed = 10f;
    public Transform projectileSpawnPoint;

    // IAttackable 인터페이스 구현 (원거리 공격)
    public override void TryAttack(float direction)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            IsAttacking = true;
            Attack(direction); // 원거리 공격 (투사체 발사)
            lastAttackTime = Time.time;
        }
    }

    // FireProjectile
    public override void Attack(float direction)
    {
        InvokeStateChangedEvent(true);
        
        GameObject projectileObj = PoolManager.Instance.ProjectilePool.GetFromPool("LineProjectile");
        IProjectile projectile = projectileObj.GetComponent<IProjectile>();
        projectileObj.transform.SetPositionAndRotation(projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        projectile.Launch(direction, float.PositiveInfinity, projectileSpeed, projectileDamage, targetLayer);

        IsAttacking = false;
    }
}
