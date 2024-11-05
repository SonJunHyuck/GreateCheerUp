using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : PlayerAttack
{
    public float attackRange = 2f; // 공격 범위
    public int attackDamage = 10; // 공격 데미지

    private void Start()
    {
        IsAttacking = false;
    }

    // 공격 시도 메서드
    public override void TryAttack(float direction)
    {
        // 쿨타임이 지난 경우에만 공격 실행
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(direction); // 공격 실행
            lastAttackTime = Time.time; // 마지막 공격 시간 갱신
            IsAttacking = true; // 공격 상태 설정
            StartCoroutine(EndAttack());
        }
    }

    // 실제 공격 실행
    public override void Attack(float direction)
    {
        InvokeStateChangedEvent(true);

        // 현재 플레이어의 방향에 따라 공격 범위를 설정
        Vector2 attackPosition = (Vector2)transform.position + (attackRange * 0.5f) * direction * Vector2.right;

        // 공격 범위 내의 적 감지
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPosition, attackRange, targetLayer);

        // 감지된 적들에게 데미지 주기
        foreach (Collider2D enemy in enemiesInRange)
        {
            // 적에게 데미지를 줄 수 있는 로직 구현 (예: EnemyHealth 스크립트)
            enemy.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
            Debug.Log($"{enemy.name}에게 {attackDamage} 데미지를 입혔습니다.");
        }
    }

    // 임시 조정
    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        IsAttacking = false;
    }

    // 공격 범위 시각화 (디버깅용)
    void OnDrawGizmosSelected()
    {
        Vector2 attackPosition = (Vector2)transform.position + Vector2.right * attackRange / 2;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
