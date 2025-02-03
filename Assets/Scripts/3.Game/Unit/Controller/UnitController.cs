using UnityEngine;
using System.Collections.Generic;
using System;

public partial class UnitController : MonoBehaviour
{
    private Animator animator;
    private IMovable movable;           // 이동
    private IAttackable attackable;     // 공격
    private IDamagable damagable;       // 체력

    private UnitAttributes attributes;

    // 군중 제어
    private Dictionary<UnitCrowdControl.CrowdControlState, Action<bool>> stateActions;
    private HashSet<UnitCrowdControl.CrowdControlState> activeStates = new();
    private UnitCrowdControl crowdControl;
    public bool IsNormalState => activeStates.Count == 0;

    public float behaviorCooldown = 0.75f;
    private float lastBehaviorTime = 0f; // 마지막 행동 시간 기록

    private bool isInit = false;

    void Start()
    {
        // IMovable 인터페이스를 구현한 컴포넌트를 가져옴
        movable = GetComponent<IMovable>();
        attackable = GetComponent<IAttackable>();
        damagable = GetComponent<IDamagable>();
        
        // 군중 제어
        crowdControl = GetComponent<UnitCrowdControl>();

        // 상태에 따른 행동을 사전(Dictionary)으로 설정
        stateActions = new Dictionary<UnitCrowdControl.CrowdControlState, Action<bool>>
        {
            { UnitCrowdControl.CrowdControlState.Freezed, Freeze },
            { UnitCrowdControl.CrowdControlState.Stun, StopAllActions },
            { UnitCrowdControl.CrowdControlState.Knockback, HittedKnockback } 
        };

        // 자식에 있는 컴포넌트
        animator = GetComponentInChildren<Animator>();

        // 애니메이션 핸들러 설정
        attackable.OnStateChanged += HandleAttackStateChanged;
        movable.OnStateChanged += HandleMovementStateChanged;
        crowdControl.OnStateChanged += HandleCrowdControlStateChanged;
        
        // 진행 방향 설정
        SetDirection();
    }

    private void OnEnable() 
    {
        lastBehaviorTime = Time.time;
        GameManager.Instance.onEndStage += HandleEndStage;
    }

    private void OnDisable()
    {
        GameManager.Instance.onEndStage -= HandleEndStage;
    }

    void Update()
    {
        if(damagable.IsDead)
        {
            return;
        }

        // 현재 상태에 맞는 행동을 딕셔너리에서 호출
        if(IsNormalState)
        {
            PerformNormalBehavior();
        }
    }

    public void Init(string key, float attack, float speed, float health)
    {
        if(!isInit)
        {
            attributes = GetComponent<UnitAttributes>();
            attributes.key = key;
            attributes.Attack = attack;
            attributes.Speed = speed;
            attributes.MaxHealth = health;

            isInit = true;
        }
    }

    private void PerformNormalBehavior()
    {
        if (Time.time < lastBehaviorTime + behaviorCooldown)
        {
            return;
        }

        // 타겟이 있고, 타겟이 공격 범위 내에 있다면
        if (attackable.HasTarget)
        {
            // 이동 취소
            animator.SetBool("IsMoving", false);

            // 공격 시도
            attackable.TryAttack(MovementDirection);

            lastBehaviorTime = Time.time;
        }
        else
        {
            movable.Move(MovementDirection);

            SetYParallelEffect();
        }
    }

    // 이동 상태 변경에 따른 애니메이션 처리
    private void HandleMovementStateChanged(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
    }

    private void Freeze(bool isStart)
    {
        Debug.Log("NPC가 아이스 상태로 행동을 멈춥니다.");

        // 아이스 상태에 따른 행동 중단 처리
        if(isStart)
        {
            animator.speed = 0.0f;
            
        }
        else
        {
            animator.speed = 1.0f;
        }
    }

    private void StopAllActions(bool isStart)
    {
        Debug.Log("NPC가 스턴 상태로 모든 행동을 중단합니다.");

        // 스턴 상태에 따른 모든 행동 중단 처리
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsStun", isStart);
    }

    private void HittedKnockback(bool isStart)
    {
        Debug.Log("NPC가 넉백 상태로 모든 행동을 중단합니다.");
        
        // 넉백 상태
        animator.SetBool("IsMoving", false);
    }

    // 공격 상태 변경에 따른 애니메이션 처리
    private void HandleAttackStateChanged(bool isAttacking)
    {
        if (isAttacking)
        {
            // 공격 애니메이션 재생
            animator.SetTrigger("DoAttack");
        }
        else
        {
            // 공격이 끝난 상태 처리
            animator.ResetTrigger("DoAttack");
        }
    }

    private void HandleCrowdControlStateChanged(UnitCrowdControl.CrowdControlState state, bool isStart)
    {
        // 죽으면 군중 제어를 받지 않음
        if(damagable.IsDead)
        {
            return;
        }

        // 군중 제어 포함 리스트
        if (isStart)
        {
            // 군중 제어 시작 시 상태 추가
            activeStates.Add(state);
        }
        else
        {
            // 군중 제어 종료 시 상태 제거
            activeStates.Remove(state);
        }

        // 군중 제어 효과에 따른 행동 시작 / 종료
        if (stateActions.TryGetValue(state, out Action<bool> action))
        {
            action?.Invoke(isStart);
        }
    }

    private void HandleEndStage()
    {
        // GetComponent<UnitController>().enabled = false;
        this.enabled = false;
    }
}
