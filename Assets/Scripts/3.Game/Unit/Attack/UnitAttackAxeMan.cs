using System.Collections;
using UnityEngine;

public class UnitAttackAxeMan : UnitAttackMelee
{
    public float stunChance = 30.0f;
    public float stunDuration = 0.5f;

    protected override sealed IEnumerator SendDamage(IDamagable damagable, float direction)
    {
        yield return attackDelay;

        if(!damagable.IsDead)
        {
            damagable.TakeDamage(attackDamage);

            if(!damagable.IsDead)
            {
                ApplyCrowdControl(damagable);
            }
        }   
    }

    private void ApplyCrowdControl(IDamagable damagable)
    {
        // 타겟이 살아 있을 때만 상태 이상 처리
        if (CurrentTarget != null && CurrentTarget.TryGetComponent<UnitCrowdControl>(out UnitCrowdControl crowdControl))
        {
            // 0부터 100 사이의 랜덤 값을 생성하여 "stunChance%" 확률로 스턴 적용
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= stunChance)
            {
                crowdControl.ApplyStun(stunDuration);
            }
        }
    }
}
