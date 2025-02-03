using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private bool isDataLoaded = false;
    public bool IsDataLoaded => isDataLoaded;

    public enum StringKey
    {
    }

    public int currentStageId;

    private Dictionary<StringKey, string> stringKeyDictionary = new();


    [Header("Stage")]
    [SerializeField] private StageInfoScriptableObject stageInfo;       // 스테이지 이름, 설명, ...
    [SerializeField] private SpawnRuleScriptableObject ruleInfo;        // 스테이지 몬스터 정보

    [Header("Ally")]
    [SerializeField] private AllyInfoScriptableObject allyUnitInfo;     // Ally 이름, 설명, Prefab, ...
    [SerializeField] private UnitGradeScriptableObject unitGradeInfo;   // Ally 단계별 공격력 등
    
    [Header("Enemy")]
    [SerializeField] private EnemyInfoScriptableObject enemyInfo;       // Enemy의 공격력, 이동속도, 골드 등

    [Header("Key")]
    [SerializeField] private NameListScriptableObject unitKeyInfo;

    public int MaxUnitGrade => 20;

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        // 게임 데이터 로드
        LoadGameData();
        
        DontDestroyOnLoad(this);
    }

    private async void LoadGameData()
    {
        Debug.Log("Initializing DataManager...");

        isDataLoaded = false; // 로드 시작 플래그 설정

        string dataLabel = "InfoData";
        var handle = Addressables.LoadResourceLocationsAsync(dataLabel); // GameData는 Addressable Label
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var location in handle.Result)
            {
                // Asset 로드
                var assetHandle = Addressables.LoadAssetAsync<ScriptableObject>(location.PrimaryKey);
                await assetHandle.Task;

                if (assetHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var asset = assetHandle.Result;

                    // ScriptableObject 타입에 따라 필드에 저장
                    if (asset is StageInfoScriptableObject stage)
                    {
                        stageInfo = stage;
                        Debug.Log($"StageInfo loaded: {location.PrimaryKey}");
                    }
                    else if (asset is SpawnRuleScriptableObject spawnRule)
                    {
                        ruleInfo = spawnRule;
                        Debug.Log($"SpawnRuleInfo loaded: {location.PrimaryKey}");
                    }
                    else if (asset is AllyInfoScriptableObject allyInfo)
                    {
                        this.allyUnitInfo = allyInfo;
                        Debug.Log($"AllyInfo loaded: {location.PrimaryKey}");
                    }
                    else if (asset is UnitGradeScriptableObject unitGrade)
                    {
                        this.unitGradeInfo = unitGrade;
                        Debug.Log($"UnitGradeInfo loaded: {location.PrimaryKey}");
                    }
                    else if (asset is EnemyInfoScriptableObject enemyInfo)
                    {
                        this.enemyInfo = enemyInfo;
                        Debug.Log($"EnemyInfo loaded: {location.PrimaryKey}");
                    }
                    else if (asset is NameListScriptableObject unitKey)
                    {
                        this.unitKeyInfo = unitKey;
                        Debug.Log($"UnitKeyInfo loaded: {location.PrimaryKey}");
                        
                        // SetUnitDictionary();
                    }
                    else
                    {
                        Debug.LogWarning($"Unknown asset type: {location.PrimaryKey}");
                    }
                }
                else
                {
                    // 실패시 재로드 시도 -> 재로드도 물가능 -> 로드 비정상 게임 종료
                    Debug.LogError($"Failed to load asset: {location.PrimaryKey}");
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load resource locations for label GameData.");
        }

        isDataLoaded = true; // 로드 완료 플래그 설정
        Debug.Log("DataManager initialization completed.");
    }

    public string GetPlayerPrefsKey(StringKey stringKey)
    {
        return stringKeyDictionary[stringKey];
    }

    // 총 스테이지 수
    public int GetStageInfoListCount()
    {
        return stageInfo.stageInfoList.Count;
    }

    // Get 특장 스테이지 정보
    public StageInfoScriptableObject.StageInfo GetStageInfo(int idx)
    {
        return stageInfo.GetStageInfo(idx);
    }

    // Get 현재 스테이지 정보
    public StageInfoScriptableObject.StageInfo GetCurrentStageInfo()
    {
        return GetStageInfo(currentStageId);
    }

    // 유닛 정보
    public AllyInfoScriptableObject.UnitInfo GetUnitInfo(string key)
    {
        return allyUnitInfo.GetUnitInfo(key);
    }

    // 특정 유닛 + 특정 등급 정보
    public UnitGradeScriptableObject.GradeInfo GetGradeInfo(string key, int grade)
    {
        // Key + Grade = PrimaryKey
        var gradeInfo = unitGradeInfo.GetGradeInfo(key, grade);
        if (gradeInfo == null)
        {
            Debug.LogWarning($"Grade {grade} not found in GradeTable {key}.");
        }
        return gradeInfo;
    }

    // 모든 유닛의 최고 능력치 정보
    public UnitGradeScriptableObject.GradeInfo GetMaxGradeInfo()
    {
        return GetGradeInfo("99_Max", 99);
    }

    public List<string> GetAllUnitKey()
    {
        List<string> UnitKeyList = new();

        foreach(AllyInfoScriptableObject.UnitInfo unitInfo in allyUnitInfo.units)
        {
            UnitKeyList.Add(unitInfo.unitKey);
        }

        return UnitKeyList;
    }

    public EnemyInfoScriptableObject.UnitInfo GetEnemyInfo(string key)
    {
        return enemyInfo.units.Find(unit => unit.unitKey == key);
    }

    // 해당 스테이지에 어떤 몬스터 등장 정보를 반환합니다. (몬스터 정보x)
    public List<SpawnRuleScriptableObject.SpawnRule> GetSpawnRule(int stageId)
    {
        List<SpawnRuleScriptableObject.SpawnRule> ruleList = new();

        foreach (var rule in ruleInfo.spawnRules)
        {
            // 해당 스테이지의 정보만 골라오기
            if(rule.stageId == stageId)
            {
                ruleList.Add(rule);
            }
        }

        return ruleList;
    }

    public int GetEnemyGold(string key)
    {
        return enemyInfo.GetUnitInfo(key).gold;
    }
}