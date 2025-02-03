using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackMagicFire : UnitAttackTargetRanged
{
    [SerializeField]
    private float explosionRadius = 5.0f;

    [SerializeField]
    private float arcHeight = 2.0f;

    protected override sealed IEnumerator DelaiedFire(float direction, float distance)
    {
        yield return attackDelay;

        // 풀에서 투사체 가져오기
        GameObject projectileObj = PoolManager.Instance.ProjectilePool.GetGameObject("ProjectileArcFire");
        projectileObj.transform.SetPositionAndRotation(projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        if(projectileObj.TryGetComponent<ProjectileArcFire>(out var projectile))
        {
            projectile.explosionRadius = explosionRadius;
            projectile.arcHeight = arcHeight;
            projectile.Launch(direction, distance, projectileSpeed, attackDamage, targetDetector.targetLayer);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Nuke);
        }
    }
}
