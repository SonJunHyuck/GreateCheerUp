using UnityEngine;

public class ProjectileLineIce : ProjectileLine
{
    public float freezeDuration = 1.0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0 && !isHitted)
        {
            isHitted = true;

            if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);

                if(!damagable.IsDead)
                {
                    Freeze(collision.gameObject);
                }
            }

            Expire();
        }
    }

    void Freeze(GameObject target)
    {
        // 폭발 이펙트 추가
        GameObject effectObj = PoolManager.Instance.EffectPool.GetFromPool("EffectIce");
        effectObj.transform.position = transform.position;

        // 얼음 효과
        if(target.activeSelf)
        {
            if (target.TryGetComponent<UnitCrowdControl>(out UnitCrowdControl crowdControl))
            {
                crowdControl.ApplyFreeze(freezeDuration);
            }
        }
    }
}
