using System;
using System.Collections;
using System.Collections.Generic;
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

    public event Action<CrowdControlState, bool> OnStateChanged;

    private Dictionary<CrowdControlState, Coroutine> activeCCCoroutines = new();
    private HitEffect hitEffect;

    void Start()
    {
        hitEffect = GetComponent<HitEffect>();
    }

    public void ApplyFreeze(float duration)
    {
        ApplyCrowdControl(CrowdControlState.Freezed, duration, HandleFreezeEffect);
    }

    public void ApplyStun(float duration)
    {
        ApplyCrowdControl(CrowdControlState.Stun, duration, HandleStunEffect);
    }

    public void ApplyKnockback(float duration, float knockbackDirection, float knockbackDistance)
    {
        ApplyCrowdControl(CrowdControlState.Knockback, duration, (d) => HandleKnockbackEffect(d, knockbackDirection, knockbackDistance));
    }

    private void ApplyCrowdControl(CrowdControlState state, float duration, Func<float, IEnumerator> effectHandler)
    {
        // 기존 상태가 활성화되어 있다면 멈춤
        if (activeCCCoroutines.TryGetValue(state, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
        }

        // 새로운 상태 효과 적용
        activeCCCoroutines[state] = StartCoroutine(EffectCoroutine(state, duration, effectHandler));
    }

    private IEnumerator EffectCoroutine(CrowdControlState state, float duration, Func<float, IEnumerator> effectHandler)
    {
        OnStateChanged?.Invoke(state, true);

        // 상태 효과 처리
        yield return effectHandler(duration);

        // 상태 종료
        OnStateChanged?.Invoke(state, false);
        activeCCCoroutines.Remove(state);
    }

    private IEnumerator HandleFreezeEffect(float duration)
    {
        if (hitEffect != null)
        {
            hitEffect.Flash(duration, Color.blue);
        }

        yield return new WaitForSeconds(duration);
    }

    private IEnumerator HandleStunEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator HandleKnockbackEffect(float duration, float knockbackDirection, float knockbackDistance)
    {
        Vector3 originalPosition = transform.position;

        // X축으로 밀어내는 거리 계산
        float knockbackX = knockbackDirection > 0 ? knockbackDistance : -knockbackDistance;
        Vector2 knockbackTargetPosition = new Vector2(Mathf.Clamp(originalPosition.x + knockbackX, -15.0f, 1.0f), originalPosition.y);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, knockbackTargetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 넉백 후에 정확히 최종 위치로 설정
        transform.position = knockbackTargetPosition;
    }
}