using System.Collections;
using UnityEngine;

public interface IProjectile
{
    void Launch(float direction, float distance, float speed, float damage, LayerMask targetLayer);
    IEnumerator DestroyAfterTime(float lifeTime);
    void Expire();
}