using System;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour, IAttackable
{
    public event Action<bool> OnStateChanged;

    public float attackCooldown = 1f; // 공격 쿨타임
    protected float lastAttackTime = 0f; // 마지막 공격 시간

    public bool HasTarget { get; }
    public bool IsAttacking { get; protected set; }
    
    public LayerMask targetLayer; // 적 레이어

    // 공격 시도 메서드
    public abstract void TryAttack(float direction);

    // 실제 공격 실행
    public abstract void Attack(float direction);

    // 이벤트를 발생시키는 메서드 (부모 클래스에서 이벤트를 Invoke, 자식 클래스 외에 사용 금지)
    protected void InvokeStateChangedEvent(bool isChanged)
    {
        OnStateChanged?.Invoke(isChanged);
    }
}
