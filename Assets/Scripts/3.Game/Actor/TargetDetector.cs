using UnityEngine;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class TargetDetector : MonoBehaviour
{
    public LayerMask targetLayer; // 타겟 레이어

    [SerializeField] private Transform currentTarget; // 현재 감지된 타겟

    // 이벤트: 타겟이 변경될 때 발생
    public event Action<Transform> OnTargetChanged;

    public Transform CurrentTarget
    {
        get => currentTarget;
        private set
        {
            if (currentTarget != value)
            {   
                currentTarget = value;

                // 타겟이 변경되면 이벤트 호출
                OnTargetChanged?.Invoke(currentTarget);
            }
        }
    }

    private void OnEnable()
    {
        currentTarget = null;
    }

    private void Start()
    {
        // Detector의 레이어를 "Default"로 설정
        gameObject.layer = 0;
    }

    public void SetRange(float radius)
    {
        // radius = attackRange
        GetComponent<CircleCollider2D>().radius = radius;
        // transform.Translate(radius, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 현재 타겟이 존재하지 않을 때만 새로운 타겟을 설정
        if (CurrentTarget == null)
        {
            // 감지된 오브젝트가 지정된 레이어에 속하는지 확인
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                bool isDead = other.GetComponent<IDamagable>().IsDead;
                if(!isDead)
                {
                    // 해당 오브젝트가 감지되면 currentTarget으로 설정
                    CurrentTarget = other.transform;
                    Debug.Log($"{other.gameObject.name}이(가) 감지되었습니다.");
                }
            }
        }
    }
    
    // 트리거에서 벗어난 오브젝트를 감지
    private void OnTriggerExit2D(Collider2D other)
    {
        CurrentTarget = null;
        Debug.Log($"{other.gameObject.name}이(가) 범위를 벗어났습니다.");

        // 부모가 비활성화 되면, 자식인 TargetDetector의 TriggerExit가 강제로 호출되면서 Coroutine을 호출
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SetNextTarget());
        }

        // // currentTarget이 범위를 벗어나면 null로 설정
        // if (other.transform == CurrentTarget)
        // {
            
        // }
    }

    private IEnumerator SetNextTarget()
    {
        yield return new WaitForEndOfFrame();

        DetectClosestTarget();
    }
    
    // 가장 가까운 적 탐색
    public bool DetectClosestTarget()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius, targetLayer);
        CurrentTarget = null; // 탐색 전 초기화
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D col in targetsInRange)
        {
            float distanceToTarget = Vector2.Distance(transform.position, col.transform.position);
            bool isDead = col.GetComponent<IDamagable>().IsDead;

            if (distanceToTarget < closestDistance && !isDead)
            {
                closestDistance = distanceToTarget;
                CurrentTarget = col.transform;
            }
        }

        // 타겟을 찾았으면 true, 못 찾았으면 false 반환
        return CurrentTarget != null;
    }
}