using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AssetCacheHandler<T> : MonoBehaviour where T : Object
{
    [SerializeField] private Dictionary<string, T> cachedAssets = new();

    /// <summary>
    /// 특정 Label로 Addressable Asset을 로드하고 캐싱
    /// </summary>
    /// <param name="label">Addressable Label</param>
    public void LoadAssetsByLabel(string label)
    {
        Addressables.LoadResourceLocationsAsync(label).Completed += OnLocationsLoaded;
    }

    /// <summary>
    /// Label로 로드한 ResourceLocations 처리
    /// </summary>
    /// <param name="handle">로드 결과 핸들</param>
    private void OnLocationsLoaded(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Found {handle.Result.Count} assets with label.");

            foreach (var location in handle.Result)
            {
                CacheAsset(location.PrimaryKey);
            }
        }
        else
        {
            Debug.LogError($"Failed to load resources with label.");
        }
    }

    /// <summary>
    /// 특정 Key에 해당하는 Asset을 캐싱
    /// </summary>
    /// <param name="key">Addressable Key</param>
    public void CacheAsset(string key)
    {
        if (cachedAssets.ContainsKey(key)) return;

        Addressables.LoadAssetAsync<T>(key).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                cachedAssets[key] = handle.Result;
                Debug.Log($"Cached asset: {key}");
            }
            else
            {
                Debug.LogError($"Failed to cache asset with key: {key}");
            }
        };
    }

    /// <summary>
    /// 캐싱된 Asset을 반환
    /// </summary>
    /// <param name="key">Addressable Key</param>
    /// <returns>캐싱된 Asset</returns>
    public T GetCachedAsset(string key)
    {
        cachedAssets.TryGetValue(key, out var asset);
        return asset;
    }
}

// Monobehavior과 제네릭을 함께 사용할 수 없기 때문에, Addcomponent<AssetCacheHandler<GameObject>>를 사용할 수 없고, 대신 이렇게 상속해서 사용
public class GameObjectCacheHandler : AssetCacheHandler<GameObject> { }