using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float damage = 10;
    public float lifeTime = 5f;

    public LayerMask targetLayer; // 타겟 레이어 지정


    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }


    private void Expire()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Expire();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            collision.GetComponent<IDamagable>()?.TakeDamage(damage);
            Expire();
        }
    }
}