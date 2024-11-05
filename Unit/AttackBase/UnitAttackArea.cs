using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackArea : UnitAttack
{
    public int maxTargets = 5;

    private readonly Collider2D[] targetsInRange = new Collider2D[10]; // 범위 내 타겟 저장용 배열

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
        InvokeStateChangedEvent(true);  // 애니메이션 실행
        
        // 공격 범위 내 모든 타겟을 탐지
        int targetsDetected = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, targetsInRange, targetDetector.targetLayer);

        // 탐지된 타겟을 리스트로 정렬 (가까운 순서대로)
        List<Collider2D> validTargets = new ();
        for (int i = 0; i < targetsDetected; i++)
        {
            validTargets.Add(targetsInRange[i]);
        }

        // 타겟들을 거리 순으로 정렬
        validTargets.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));

        // 데미지 일괄처리를 위한 배열
        int targetCount = Mathf.Min(maxTargets, validTargets.Count);
        IDamagable[] damageTargets = new IDamagable[targetCount];

        // 최대 타겟 수에 맞춰 공격
        for (int i = 0; i < targetCount; i++)
        {
            if (validTargets[i].TryGetComponent<IDamagable>(out var damagable))
            {
                damageTargets[i] = damagable;
            }
        }

        StartCoroutine(SendDamage(damageTargets, direction, targetCount));
    }

    protected abstract IEnumerator SendDamage(IDamagable[] damagables, float direction, int targetCount);
}
