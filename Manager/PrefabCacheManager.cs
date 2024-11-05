using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PrefabCacheManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<string, GameObject> cachedPrefabs = new();

    public async void LoadAssetsByLabel(string label)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        Debug.Log($"Found {locations.Count} assets with label {label}:");

        foreach (var location in locations)
        {
            await CachePrefab(location.PrimaryKey);
        }
    }

    // 캐싱된 프리팹을 가져오는 메서드
    public GameObject GetCachedPrefab(string key)
    {
        cachedPrefabs.TryGetValue(key, out var prefab);
        return prefab;
    }

    // 특정 key에 해당하는 프리팹을 캐싱
    public async Task CachePrefab(string key)
    {
        if (cachedPrefabs.ContainsKey(key)) return;

        var assetReference = new AssetReferenceGameObject(key);
        var handle = assetReference.LoadAssetAsync<GameObject>();
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cachedPrefabs[key] = handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load prefab with addressable key: {key}");
        }
    }

    public async Task<GameObject> LoadPrefabAsync(string key)
    {
        if (cachedPrefabs.ContainsKey(key))
        {
            return cachedPrefabs[key];
        }

        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cachedPrefabs[key] = handle.Result;
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load prefab asynchronously with key: {key}");
            return null;
        }
    }
}