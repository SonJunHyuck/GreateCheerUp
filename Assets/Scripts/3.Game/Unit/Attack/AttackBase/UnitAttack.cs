using UnityEngine;

public abstract class UnitAttack : MonoBehaviour, IAttackable
{
    // Interface
    public event System.Action<bool> OnStateChanged;
    public bool HasTarget => CurrentTarget != null;

    // 타겟
    protected TargetDetector targetDetector; // 타겟 탐지 클래스 참조
    protected Transform CurrentTarget => targetDetector.CurrentTarget;

    // 공격 옵션
    public float attackDamage = 10f;
    public float attackRange = 2f; // 공격 범위
    public float attackCooldown = 1f;
    protected float lastAttackTime = 0f; // 마지막 공격 시간 기록

    // Delay
    public float attackDelayDuration = 0.2f;
    protected WaitForSeconds attackDelay;

    protected virtual void Start()
    {
        targetDetector = GetComponentInChildren<TargetDetector>();
        targetDetector.SetRange(attackRange);
        targetDetector.OnTargetChanged += HandleTargetChanged;  // 타겟 변경 이벤트 구독

        attackDelay = new WaitForSeconds(attackDelayDuration);
    }

    // 기본 구현이 있지만, 재정의 허용
    public virtual void TryAttack(float direction)
    {
        // 공격 쿨타임 조건
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if(CurrentTarget.GetComponent<IDamagable>().IsDead)
            {
                targetDetector.DetectClosestTarget();
                return;
            }

            Attack(direction);
            lastAttackTime = Time.time;
        }
    }

    // 공격 실행 메서드
    public abstract void Attack(float direction);

    // 이벤트를 발생시키는 메서드 (부모 클래스에서 이벤트를 Invoke, 자식 클래스 외에 사용 금지)
    protected void InvokeStateChangedEvent(bool isChanged)
    {
        OnStateChanged?.Invoke(isChanged);
    }

    // 타겟 변경 시 호출되는 메서드 (null이라도 알려줘야함)
    private void HandleTargetChanged(Transform newTarget)
    {
        if (newTarget != null)
        {
            Debug.Log($"{newTarget.gameObject.name}이(가) 새 타겟으로 설정되었습니다.");
        }
        else
        {
            Debug.Log(gameObject.name + "타겟이 범위를 벗어났습니다.");
        }
    }

    void OnDrawGizmos()
    {
        // Gizmo 색상 설정
        Gizmos.color = Color.red;

        // 위치와 반경을 이용해 원 형태의 Gizmo 그리기
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
