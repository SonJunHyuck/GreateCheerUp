using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InGameUIManager : MonoBehaviour
{   
    private void Start()
    {
        InitializeAllySpawnButtons();
    }

    #region Ally Spawn
    [Header("Ally Spawn Button")]
    private List<string> allySpriteKeys = new List<string>(); // Addressable에서 가져온 Ally Sprite Key 리스트
    [SerializeField] private readonly string spriteLabel = "SpriteAlly"; // Addressable Label (Ally Sprite Keys 그룹)
    [SerializeField] private GameObject buttonPrefab;   // AllySpawnButton 프리팹
    [SerializeField] private Transform buttonContainer; // 버튼들이 배치될 부모 Transform

    // 버튼 초기화를 시작합니다.
    public void InitializeAllySpawnButtons()
    {
        LoadAllyKeys();
    }

    // Addressable Label을 이용해 Ally Key들을 로드합니다.
    private void LoadAllyKeys()
    {
        Addressables.LoadResourceLocationsAsync(spriteLabel).Completed += OnSpriteKeysLoaded;
    }

    // Ally Keys 로드 완료 콜백 함수
    private void OnSpriteKeysLoaded(AsyncOperationHandle<IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var location in handle.Result)
            {
                if (allySpriteKeys.Contains(location.PrimaryKey))
                {
                    Debug.LogWarning($"Duplicate Key Detected: {location.PrimaryKey}");
                    continue;
                }

                allySpriteKeys.Add(location.PrimaryKey);
            }

            StartCoroutine(CreateAndInitializeButtons());
        }
        else
        {
            Debug.LogError($"Failed to load ally keys for label: {spriteLabel}");
        }
    }

    // AllySpawnButton 프리팹을 생성하고 초기화
    private IEnumerator CreateAndInitializeButtons()
    {
        allySpriteKeys.Sort();
        int num = 1;
        foreach (var key in allySpriteKeys)
        {
            // Addressable에서 Sprite 로드
            var spriteHandle = Addressables.LoadAssetAsync<Sprite>(key);
            yield return spriteHandle;

            if (spriteHandle.Status == AsyncOperationStatus.Succeeded)
            {
                // 버튼 생성
                GameObject button = Instantiate(buttonPrefab, buttonContainer);

                // AllySpawnButton 스크립트 초기화
                ButtonAllySpawn allyButton = button.GetComponent<ButtonAllySpawn>();

                string allyKey = key.Replace("Sprite", "");
                int cost = DataManager.Instance.GetUnitInfo(allyKey).cost;
                allyButton.Initialize(allyKey, cost, spriteHandle.Result, num++); // Key와 Sprite 전달
            }
            else
            {
                Debug.LogError($"Failed to load sprite for key: {key}");
            }
        }
    }
    #endregion
}