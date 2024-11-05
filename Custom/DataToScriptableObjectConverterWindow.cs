using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class DataToScriptableObjectConverterWindow : EditorWindow
{
    private string csvFilePath = "Assets/Data/data.csv"; // CSV 파일 경로
    private string assetSavePath = "Assets/Data/"; // ScriptableObject 저장 경로
    private ScriptableObject selectedScriptableObject; // 사용자가 선택한 ScriptableObject

    [MenuItem("Tools/Custom Data Converter")]
    public static void ShowWindow()
    {
        GetWindow<DataToScriptableObjectConverterWindow>("Custom Data Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV Data to ScriptableObject Converter", EditorStyles.boldLabel);

        // CSV 파일 경로 입력
        EditorGUILayout.LabelField("CSV File Path");
        csvFilePath = EditorGUILayout.TextField(csvFilePath);

        // ScriptableObject 저장 경로 입력
        EditorGUILayout.LabelField("Asset Save Path");
        assetSavePath = EditorGUILayout.TextField(assetSavePath);

        // ScriptableObject 선택
        EditorGUILayout.LabelField("Target ScriptableObject Type");
        selectedScriptableObject = EditorGUILayout.ObjectField("ScriptableObject", selectedScriptableObject, typeof(ScriptableObject), false) as ScriptableObject;

        // CSV to ScriptableObject 변환 버튼
        if (GUILayout.Button("Convert CSV to ScriptableObject"))
        {
            ConvertCSVToScriptableObject();
        }
    }

    // CSV 파일을 읽고 선택한 ScriptableObject의 타입에 맞춰서 데이터를 변환하는 메서드
    private void ConvertCSVToScriptableObject()
    {
        if (!File.Exists(csvFilePath))
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + csvFilePath);
            return;
        }

        if (selectedScriptableObject == null)
        {
            Debug.LogError("ScriptableObject를 선택해주세요.");
            return;
        }

        // CSV 파일 읽기
        string[] csvData = File.ReadAllLines(csvFilePath);
        if (csvData.Length == 0)
        {
            Debug.LogError("CSV 파일이 비어 있습니다.");
            return;
        }

        // 첫 번째 줄은 헤더로 설정
        string[] headers = csvData[0].Split(',');

        // 두 번째 줄부터 데이터를 처리 (헤더 이후의 줄)
        for (int i = 1; i < csvData.Length; i++)
        {
            string[] rowData = csvData[i].Split(',');

            if (rowData.Length != headers.Length)
            {
                Debug.LogError($"CSV 데이터가 잘못되었습니다. 열 개수가 맞지 않습니다. 줄 번호: {i + 1}");
                continue;
            }

            // 선택한 ScriptableObject의 타입에 맞춰 데이터를 변환
            if (selectedScriptableObject is AddressableAssetScriptableObject)
            {
                ConvertToAddressableAssetSO(rowData);
            }
            else if (selectedScriptableObject is EnemySpawnRuleScriptableObject)
            {
                ConvertToEnemySpawnRuleSO(rowData);
            }
            else if(selectedScriptableObject is PoolPresetScriptableObject)
            {
                ConvertToPoolPresetSO(rowData);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("CSV에서 ScriptableObject로 변환 완료.");
    }

    // CSV 데이터를 AddressableAssetScriptableObject로 변환하여 List에 저장
    private void ConvertToAddressableAssetSO(string[] rowData)
    {
        // 선택한 ScriptableObject를 AddressableAssetScriptableObject로 캐스팅
        var asset = selectedScriptableObject as AddressableAssetScriptableObject;

        if (asset == null)
        {
            Debug.LogError("AddressableAssetScriptableObject로 캐스팅할 수 없습니다.");
            return;
        }

        // 새로운 AddressableAssetInfo를 생성하여 데이터를 추가
        AddressableAssetScriptableObject.AddressableAssetInfo assetInfo = new AddressableAssetScriptableObject.AddressableAssetInfo
        {
            addressableKey = rowData[0], // 첫 번째 열은 addressableKey
            tag = rowData[1]             // 두 번째 열은 tag
        };

        // List에 추가
        asset.addressableAssetInfoList.Add(assetInfo);
        Debug.Log($"AddressableAssetInfo 추가됨: {assetInfo.addressableKey} - {assetInfo.tag}");

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }

    private void ConvertToPoolPresetSO(string[] rowData)
    {
        // 선택한 ScriptableObject를 PoolPresetScriptableObject로 캐스팅
        var poolPreset = selectedScriptableObject as PoolPresetScriptableObject;

        if (poolPreset == null)
        {
            Debug.LogError("PoolPresetScriptableObject로 캐스팅할 수 없습니다.");
            return;
        }

        // 새로운 PoolPresetInfo 생성하여 데이터를 추가
        PoolPresetScriptableObject.PoolPresetInfo presetInfo = new PoolPresetScriptableObject.PoolPresetInfo
        {
            tag = rowData[0],             // 첫 번째 열은 tag
            size = int.Parse(rowData[1])
        };

        // List에 추가
        poolPreset.poolPresetInfoList.Add(presetInfo);
        Debug.Log($"AddressableAssetInfo 추가됨: {presetInfo.tag}");

        // ScriptableObject 저장
        EditorUtility.SetDirty(poolPreset); // 변경 사항 저장
    }

    // CSV 데이터를 EnemySpawnRuleScriptableObject로 변환하여 List에 저장
    private void ConvertToEnemySpawnRuleSO(string[] rowData)
    {
        // 선택한 ScriptableObject를 EnemySpawnRuleScriptableObject로 캐스팅
        var asset = selectedScriptableObject as EnemySpawnRuleScriptableObject;

        if (asset == null)
        {
            Debug.LogError("EnemySpawnRuleScriptableObject로 캐스팅할 수 없습니다.");
            return;
        }

        // 스테이지 ID를 가져옴 (첫 번째 열)
        int stageId = int.Parse(rowData[0]);

        // 새로운 EnemySpawnRule을 생성하여 데이터를 추가
        EnemySpawnRuleScriptableObject.EnemySpawnRule spawnRule = new EnemySpawnRuleScriptableObject.EnemySpawnRule
        {
            tag = rowData[1],             // 유닛 태그 (두 번째 열)
            spawnCount = int.Parse(rowData[2]),   // 스폰 횟수 (세 번째 열)
            minSpawnCount = int.Parse(rowData[3]),// 최소 스폰 횟수 (네 번째 열)
            maxSpawnCount = int.Parse(rowData[4]),// 최대 스폰 횟수 (다섯 번째 열)
            minInterval = float.Parse(rowData[5]),// 최소 스폰 간격 (여섯 번째 열)
            maxInterval = float.Parse(rowData[6]) // 최대 스폰 간격 (일곱 번째 열)
        };

        // 해당 스테이지 ID가 동일한 경우에만 추가
        if (asset.stageId == stageId)
        {
            asset.spawnRules.Add(spawnRule);
            Debug.Log($"Stage {stageId}에 유닛 스폰 규칙 추가됨: {spawnRule.tag}");
        }
        else
        {
            Debug.LogWarning($"Stage ID {stageId}가 {asset.stageId}와 일치하지 않습니다. 건너뜀.");
        }

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }
}