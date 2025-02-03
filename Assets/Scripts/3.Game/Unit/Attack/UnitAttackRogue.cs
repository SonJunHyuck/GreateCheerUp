using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackRogue : UnitAttackMultiHit
{
    protected override sealed IEnumerator SendDamage(IDamagable damagable, float direction)
    {
        yield return attackDelay;

        for (int i = 0; i < hitCount; i++)
        {
            if (damagable != null && !damagable.IsDead)
            {
                damagable.TakeDamage(attackDamage);

                // 타겟이 사망했으면 루프 종료
                if (damagable.IsDead)
                {
                    break;
                }

                // 다음 히트 전 대기
                yield return multiHitDelay;
            }
            else
            {
                // 타겟이 사망했거나 사라진 경우 루프 종료
                break;
            }
        }
    }
}
