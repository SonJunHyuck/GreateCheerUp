using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableUpdater : MonoBehaviour
{
    public static AddressableUpdater Instance { get; private set; }

    public event Action<float> OnDownloadProgressChanged; // 진행률 이벤트
    public event Action OnDownloadComplete; // 완료 이벤트

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public IEnumerator CheckAndDownloadAddressables()
    {
        yield return Addressables.InitializeAsync(); // Addressables 초기화

        // Catalog 업데이트 확인
        var checkHandle = Addressables.CheckForCatalogUpdates();
        yield return checkHandle;

        if (checkHandle.Status == AsyncOperationStatus.Succeeded && checkHandle.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result);
            yield return updateHandle;
            Addressables.Release(updateHandle);
        }
        Addressables.Release(checkHandle);

        // 변경된 리소스 다운로드
        yield return DownloadAddressables();
    }

    private IEnumerator DownloadAddressables()
    {
        var getSizeHandle = Addressables.GetDownloadSizeAsync("Default");
        yield return getSizeHandle;

        long totalDownloadSize = getSizeHandle.Result;
        Addressables.Release(getSizeHandle);

        if (totalDownloadSize == 0)
        {
            OnDownloadComplete?.Invoke();
            yield break;
        }

        var downloadHandle = Addressables.DownloadDependenciesAsync("Default", true);
        while (!downloadHandle.IsDone)
        {
            OnDownloadProgressChanged?.Invoke(downloadHandle.PercentComplete);
            yield return null;
        }

        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            OnDownloadComplete?.Invoke();
        }
        else
        {
            Debug.LogError("Download failed!");
        }
        Addressables.Release(downloadHandle);
    }
}