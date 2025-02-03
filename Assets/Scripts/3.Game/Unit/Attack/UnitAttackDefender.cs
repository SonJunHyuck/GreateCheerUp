using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackDefender : UnitAttackMelee
{
    protected override sealed IEnumerator SendDamage(IDamagable damagable, float direction)
    {
        yield return attackDelay;

        // Damage를 주지 않음
    }
}
