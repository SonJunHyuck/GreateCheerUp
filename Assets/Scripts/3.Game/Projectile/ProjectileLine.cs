using UnityEngine;
using System.Collections;

public class ProjectileLine : MonoBehaviour, IProjectile
{
    public float projectileSpeed = 10f;
    private Vector3 direction;
    private bool isLaunched = false;
    protected bool isHitted = false;
    public float damage = 10f;
    public LayerMask targetLayer; // 타겟 레이어 지정
    // 직선 발사 Update 메서드
    private void Update()
    {
        if (isLaunched)
        {
            transform.localPosition += projectileSpeed * Time.deltaTime * direction;
        }
    }

    // Launch 메서드 구현
    public void Launch(float direction, float distance, float speed, float damage, LayerMask targetLayer)
    {
        // distance 무시
        
        this.targetLayer = targetLayer;
        this.damage = damage;
        this.direction = new Vector3(direction, 0, 0).normalized;
        projectileSpeed = speed;
        isLaunched = true;
        isHitted = false;
    }

    // DestroyAfterTime 메서드 구현
    public IEnumerator DestroyAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Expire();
    }

    // Expire 메서드 구현
    public void Expire()
    {
        isLaunched = false;
        gameObject.SetActive(false); // 풀로 반환
    }

    // 충돌 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0 && !isHitted)
        {
            isHitted = true;
            collision.GetComponent<IDamagable>()?.TakeDamage(damage);
            Expire();
        }
    }
}