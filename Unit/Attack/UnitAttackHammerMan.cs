using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackHammerMan : UnitAttackMelee
{
    public float knockbackDistanceX = 5f; // 넉백 힘
    public float knockbackDuration = 0.5f;

    protected override sealed IEnumerator SendDamage(IDamagable damagable, float direction)
    {
        yield return attackDelay;

        if(!damagable.IsDead)
        {
            damagable.TakeDamage(attackDamage);

            if(!damagable.IsDead)
            {
                ApplyCrowdControl(damagable, direction);
            }
        }
    }

    private void ApplyCrowdControl(IDamagable damagable, float direction)
    {
        if(CurrentTarget != null)
        {
            // 죽지 않았을 때, 상태이상 처리
            if (CurrentTarget.TryGetComponent<UnitCrowdControl>(out UnitCrowdControl crowdControl))
            {
                crowdControl.ApplyKnockback(knockbackDuration, direction, knockbackDistanceX);
            }

            Debug.Log($"{CurrentTarget.gameObject.name}에게 {attackDamage}의 피해를 입혔습니다.");
        }
    }
}
