using UnityEngine;
using System.Collections;

public class ProjectileArc : MonoBehaviour, IProjectile
{
    public float projectileSpeed = 10f;
    public float arcHeight = 2f;

    protected bool isLaunched = false;
    protected Vector2 startPoint;
    protected Vector2 targetPoint;
    protected float launchStartTime; // 발사 시점 기록

    protected float damage = 10f;
    public LayerMask targetLayer; // 타겟 레이어 지정

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
                Expire();
            }
        }
    }

    // Launch 메서드 구현
    public void Launch(float direction, float distance, float speed, float damage, LayerMask targetLayer)
    {
        this.projectileSpeed = speed;
        this.targetLayer = targetLayer;
        this.damage = damage;
        startPoint = transform.localPosition;
        targetPoint = startPoint + new Vector2(direction * distance, 0);
        isLaunched = true;

        // 발사 시점 기록
        launchStartTime = Time.time;
    }

    // DestroyAfterTime 메서드 구현
    public IEnumerator DestroyAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Expire();
    }

    // Expire 메서드 구현
    public virtual void Expire()
    {
        isLaunched = false;
        gameObject.SetActive(false); // 풀로 반환
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            collision.GetComponent<IDamagable>()?.TakeDamage(damage);
            Expire();
        }
    }
}