using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PoolManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("PoolManager");
                    instance = singleton.AddComponent<PoolManager>();
                }
            }

            return instance;
        }
    }

    private Dictionary<string, ObjectPool> pools;
    public ObjectPool UnitPool
    {
        get { return pools["Unit"]; }
    }

    public ObjectPool ProjectilePool
    {
        get { return pools["Projectile"]; }
    }

    public ObjectPool EffectPool
    {
        get { return pools["Effect"]; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void InitPoolManager()
    {
        pools = new();
        
        CreatePool("Unit");
        CreatePool("Projectile");
        CreatePool("Effect");
    }

    private void CreatePool(string assetsLabel)
    {
        GameObject pool = new GameObject(assetsLabel);
        pool.transform.SetParent(this.transform); // GameManager 하위에 추가
        
        ObjectPool objectPool = pool.AddComponent<ObjectPool>();
        objectPool.InitObjectPool(assetsLabel);

        pools.Add(assetsLabel, objectPool);
    }
}