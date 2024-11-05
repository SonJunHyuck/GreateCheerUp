using System;
using System.Collections;
using UnityEngine;

public class UnitCrowdControl : MonoBehaviour
{
    // 군중 제어 상태 enum
    public enum CrowdControlState
    {
        Normal,
        Freezed,
        Stun,
        Knockback
    }

    public CrowdControlState currentState = CrowdControlState.Normal; // 현재 상태를 저장하는 변수
    
    public event Action<CrowdControlState, bool> OnStateChanged;

    private Coroutine ccCoroutine;

    private HitEffect hitEffect;

    void Start()
    {
        hitEffect = GetComponent<HitEffect>();
    }

    public void ApplyFreeze(float duration)
    {
        if (ccCoroutine != null)
        {
            StopCoroutine(ccCoroutine);
        }
        ccCoroutine = StartCoroutine(HandleFreezeEffect(duration));
    }

    public void ApplyStun(float duration)
    {
        if (ccCoroutine != null)
        {
            StopCoroutine(ccCoroutine);
        }
        ccCoroutine = StartCoroutine(HandleStunEffect(duration));
    }

    public void ApplyKnockback(float duration, float knockbackDirection, float konckbackDistance)
    {
        if (ccCoroutine != null)
        {
            StopCoroutine(ccCoroutine);
        }
        ccCoroutine = StartCoroutine(HandleKnockbackEffect(duration, knockbackDirection, konckbackDistance));
    }

    // 아이스 처리 코루틴
    private IEnumerator HandleFreezeEffect(float duration)
    {
        currentState = CrowdControlState.Freezed;
        OnStateChanged.Invoke(CrowdControlState.Freezed, true);
        hitEffect.Flash(duration, Color.blue);

        yield return new WaitForSeconds(duration);

        currentState = CrowdControlState.Normal; // 상태 복귀
        OnStateChanged.Invoke(CrowdControlState.Freezed, false);  // Controller에 Freeze 해제 전달
    }

    // 스턴 처리 코루틴
    private IEnumerator HandleStunEffect(float duration)
    {
        currentState = CrowdControlState.Stun;
        OnStateChanged.Invoke(CrowdControlState.Stun, true);

        yield return new WaitForSeconds(duration);
        
        currentState = CrowdControlState.Normal; // 상태 복귀
        OnStateChanged.Invoke(CrowdControlState.Stun, false);
    }

    private IEnumerator HandleKnockbackEffect(float duration, float knockbackDirection, float konckbackDistance)
    {
        currentState = CrowdControlState.Knockback;
        OnStateChanged.Invoke(CrowdControlState.Knockback, true);

        Vector3 originalPosition = transform.position;

        // X축으로 밀어내는 거리 계산
        float knockbackX = knockbackDirection > 0 ? konckbackDistance : -konckbackDistance;

        Vector2 knockbackTargetPosition = new Vector2(originalPosition.x + knockbackX, originalPosition.y);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, knockbackTargetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 넉백 후에 정확히 최종 위치로 설정
        transform.position = knockbackTargetPosition;

        currentState = CrowdControlState.Normal; // 상태 복귀
        OnStateChanged.Invoke(CrowdControlState.Knockback, false);
    }
}
