using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArcFire : ProjectileArc
{
    public float explosionRadius = 5f; // 폭발 범위

    private bool hasExploded = false; // 중복 데미지 방지 플래그

    // 곡선 발사 Update 메서드
    private void Update()
    {
        if (isLaunched)
        {
            // 경과 시간을 계산 (현재 시간 - 발사 시점)
            float elapsedTime = Time.time - launchStartTime;
            float progress = Mathf.Clamp01(elapsedTime * projectileSpeed); // 경과 시간과 속도에 따른 진행 비율
            float heightAdjustment = Mathf.Sin(progress * Mathf.PI) * arcHeight;

            // X축과 Y축 계산
            Vector2 currentPosition = Vector2.Lerp(startPoint, targetPoint, progress);
            currentPosition.y += heightAdjustment;

            transform.localPosition = currentPosition;

            if (progress >= 1f)
            {
                Explode();
            }
        }
    }

    // 폭발 메서드: 일정 범위 내 모든 적에게 데미지
    private void Explode()
    {
        if(hasExploded)
        {
            return;
        }
        
        hasExploded = true; // 중복 처리를 방지

        // 범위 내의 적들을 탐지
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

        // 탐지된 적들에게 데미지 적용
        foreach (Collider2D target in hitTargets)
        {
            ApplyDamage(target.transform);
        }

        // 폭발 이펙트 추가
        GameObject effectObj = PoolManager.Instance.EffectPool.GetGameObject("EffectFire");
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Explosion);
        effectObj.transform.position = transform.position;

        // 투사체 소멸
        Expire();
    }
    
    public override void Expire()
    {
        hasExploded = false;
        base.Expire();
    }

    private void ApplyDamage(Transform target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        damagable?.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            collision.GetComponent<IDamagable>()?.TakeDamage(damage);
            Explode();
        }
    }
}
