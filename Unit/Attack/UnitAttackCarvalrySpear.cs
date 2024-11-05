using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackCarvalrySpear : UnitAttackArea
{
    protected override sealed IEnumerator SendDamage(IDamagable[] damagables, float direction, int targetCount)
    {
        yield return attackDelay;

        for (int i = 0; i < targetCount; i++)
        {
            if(damagables[i] != null && !damagables[i].IsDead)
            {
                damagables[i].TakeDamage(attackDamage);

                // *** 여기서 currentTarget이 죽으면, currentTarget == null ***
                if (CurrentTarget != null)
                {
                    Debug.Log($"{CurrentTarget.gameObject.name}에게 {attackDamage}의 피해를 입혔습니다.");
                }
            }
        }
    }
}
