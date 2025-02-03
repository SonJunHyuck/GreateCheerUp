using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, List<GameObject>> poolDictionary = new ();

    private GameObjectCacheHandler objectCacheManager;

    private bool isSuccessInit = false;

    public void InitObjectPool(string label)
    {
        objectCacheManager = transform.AddComponent<GameObjectCacheHandler>();
        objectCacheManager.LoadAssetsByLabel(label);

        // Pool 초기화 완료
        isSuccessInit = true;
    }

    // 특정 Name에 해당하는 오브젝트를 풀에서 가져오는 메서드
    public GameObject GetGameObject(string key)
    {
        if(!isSuccessInit)
        {
            DebugWrapper.LogWarning($"Initializing Object Pool is not complete : {key}");
            return null;
        }

        // Dictionary에 풀을 초기화
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new List<GameObject>();
        }
        
        // 풀에 사용 가능한 오브젝트가 있는지 확인
        foreach (GameObject obj in poolDictionary[key])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 없으면 만들어서 반환
        return CreateNewObject(key);
    }

    // 새로운 오브젝트를 생성하고 Dictionary에 추가하는 코루틴
    GameObject CreateNewObject(string key)
    {
        GameObject prefab = objectCacheManager.GetCachedAsset(key);
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab with key: {key}");
            // 기본 프리펩을 반환하기
            return null;
        }

        // 새로운 오브젝트 생성 및 Dictionary에 추가
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        newObj.transform.SetParent(transform);

        newObj.GetComponent<IInitializable>()?.Init(key);

        poolDictionary[key].Add(newObj);
        return newObj;
    }
}