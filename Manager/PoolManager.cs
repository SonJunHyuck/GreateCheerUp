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
                    DontDestroyOnLoad(singleton);
                }
            }

            return instance;
        }
    }

    public Dictionary<string, ObjectPool> pools;
    public ObjectPool AllyPool
    {
        get { return pools["Ally"]; }
    }

    public ObjectPool EnemyPool
    {
        get { return pools["Enemy"]; }
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
            DontDestroyOnLoad(gameObject);
        }

        pools = new();
        InitializeObjectPools();
    }

    // ObjectPool 초기화 로직
    private void InitializeObjectPools()
    {
        CreatePool("Ally");
        CreatePool("Enemy");
        CreatePool("Projectile");
        CreatePool("Effect");
    }

    void CreatePool(string poolName)
    {
        GameObject pool = new GameObject(poolName);
        pool.transform.SetParent(this.transform); // GameManager 하위에 추가
        ObjectPool objectPool = pool.AddComponent<ObjectPool>();
        objectPool.InitObjectPool(poolName);
        pools.Add(poolName, objectPool);
    }
}