using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackMagicIce : UnitAttackRanged
{
    [SerializeField]
    private float freezeDuration = 1.0f;

    protected override sealed IEnumerator DelaiedFire(float direction)
    {
        yield return attackDelay;

        // 풀에서 투사체 가져오기
        GameObject projectileObj = PoolManager.Instance.ProjectilePool.GetGameObject("ProjectileLineIce");
        projectileObj.transform.SetPositionAndRotation(projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        if(projectileObj.TryGetComponent<ProjectileLineIce>(out var projectile))
        {
            projectile.freezeDuration = freezeDuration;
            projectile.Launch(direction, float.PositiveInfinity, projectileSpeed, attackDamage, targetDetector.targetLayer);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Nuke);
        }
    }
}
