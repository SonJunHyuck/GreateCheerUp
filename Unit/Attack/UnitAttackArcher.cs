using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackArcher : UnitAttackRanged
{
    protected override sealed IEnumerator DelaiedFire(float direction)
    {
        yield return attackDelay;

        // 풀에서 투사체 가져오기
        GameObject projectileObj = PoolManager.Instance.ProjectilePool.GetFromPool("ProjectileArrow");
        projectileObj.transform.SetPositionAndRotation(projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        
        if(projectileObj.TryGetComponent<ProjectileLine>(out var projectile))
        {
            projectile.Launch(direction, float.PositiveInfinity, projectileSpeed, attackDamage, targetDetector.targetLayer);
        }
    }
}
