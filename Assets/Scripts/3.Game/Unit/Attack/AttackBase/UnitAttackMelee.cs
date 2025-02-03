using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackMelee : UnitAttack
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TryAttack(float direction)
    {
        base.TryAttack(direction);
    }

    // 공격 실행 메서드
    public override void Attack(float direction)
    {
        if(CurrentTarget.TryGetComponent<IDamagable>(out var damagable))
        {   
            InvokeStateChangedEvent(true);  // 애니메이션 실행
            StartCoroutine(SendDamage(damagable, direction));
        }
    }

    protected abstract IEnumerator SendDamage(IDamagable damagable, float direction);
}
