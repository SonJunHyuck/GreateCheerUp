using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.MPE;

public class DataToScriptableObjectConverterWindow : EditorWindow
{
    private string csvFilePath = "Assets/Data/data.csv"; // CSV 파일 경로

    private ScriptableObject selectedScriptableObject; // 사용자가 선택한 ScriptableObject
    private bool isDragAreaActive = false; // 드래그 영역 상태
    private bool isCsvFileValid = false; // CSV 파일 유효성 상태

    [MenuItem("Tools/Custom Data Converter")]
    public static void ShowWindow()
    {
        GetWindow<DataToScriptableObjectConverterWindow>("Custom Data Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV Data to ScriptableObject Converter", EditorStyles.boldLabel);

        // 드래그 앤 드롭 영역 추가
        DrawDragAndDropArea();

        // ScriptableObject 선택
        GUILayout.Label("Target ScriptableObject Type");
        selectedScriptableObject = EditorGUILayout.ObjectField("ScriptableObject", selectedScriptableObject, typeof(ScriptableObject), false) as ScriptableObject;

        // CSV to ScriptableObject 변환 버튼
        if (GUILayout.Button("Convert CSV to ScriptableObject"))
        {
            ConvertCSVToScriptableObject();
        }
    }

    private void DrawDragAndDropArea()
    {
        // 드래그 앤 드롭 영역을 그리는 Rect 설정
        Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag & Drop CSV File Here", EditorStyles.helpBox);

        // 드래그 이벤트 처리
        Event evt = Event.current;
        if (dropArea.Contains(evt.mousePosition))
        {
            switch (evt.type)
            {
                case EventType.DragUpdated: // 드래그 중 업데이트
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    isDragAreaActive = true;
                    evt.Use();
                    break;

                case EventType.DragPerform: // 드래그 후 드롭
                    DragAndDrop.AcceptDrag();
                    if (DragAndDrop.paths.Length > 0)
                    {
                        string path = DragAndDrop.paths[0];
                        if (Path.GetExtension(path).Equals(".csv"))
                        {
                            csvFilePath = path;
                            isCsvFileValid = true; // CSV 파일 유효
                            Debug.Log($"CSV 파일 경로 설정: {csvFilePath}");
                        }
                        else
                        {
                            isCsvFileValid = false; // CSV 파일이 아님
                            Debug.LogError("CSV 파일만 드롭할 수 있습니다.");
                        }
                    }
                    evt.Use();
    break;
            }
        }
        else if (isDragAreaActive && evt.type == EventType.DragExited)
        {
            isDragAreaActive = false;
        }

        // 현재 설정된 CSV 파일 경로 표시
        GUILayout.Label("CSV File Path:");
        GUIStyle pathStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            normal = { textColor = isCsvFileValid ? Color.green : Color.red } // 유효하면 녹색, 아니면 빨간색
        };
        GUILayout.Label(csvFilePath, pathStyle);

        if (isCsvFileValid)
        {
            GUILayout.Label("CSV 파일이 유효합니다.", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("유효한 CSV 파일을 드래그 앤 드롭해주세요.", EditorStyles.boldLabel);
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

        InitializeListForScriptableObject();

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
            if (selectedScriptableObject is SpawnRuleScriptableObject)
            {
                ConvertToSpawnRuleSO(rowData);
            }
            else if (selectedScriptableObject is PoolPresetScriptableObject)
            {
                ConvertToPoolPresetSO(rowData);
            }
            else if (selectedScriptableObject is StageInfoScriptableObject)
            {
                ConvertToStageInfoSO(rowData);
            }
            else if (selectedScriptableObject is AllyInfoScriptableObject)
            {
                ConvertToUnitInfoModel(rowData);
            }
            else if (selectedScriptableObject is UnitGradeScriptableObject)
            {
                ConvertToUnitGrade(rowData);
            }
            else if(selectedScriptableObject is EnemyInfoScriptableObject)
            {
                ConvertToEnemyInfo(rowData);
            }
            else if(selectedScriptableObject is LoadingMessageScriptableObject)
            {
                ConvertToLoadingMessage(rowData);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("CSV에서 ScriptableObject로 변환 완료.");
    }

    private void InitializeListForScriptableObject()
    {
        if (selectedScriptableObject is SpawnRuleScriptableObject spawnRule)
        {
            spawnRule.spawnRules?.Clear();
            Debug.Log("SpawnRuleScriptableObject의 리스트를 초기화했습니다.");
        }
        else if (selectedScriptableObject is PoolPresetScriptableObject poolPreset)
        {
            poolPreset.poolPresetInfoList?.Clear();
            Debug.Log("PoolPresetScriptableObject의 리스트를 초기화했습니다.");
        }
        else if (selectedScriptableObject is StageInfoScriptableObject stageInfo)
        {
            stageInfo.stageInfoList?.Clear();
            Debug.Log("StageInfoScriptableObject의 리스트를 초기화했습니다.");
        }
        else if (selectedScriptableObject is AllyInfoScriptableObject unitInfoModel)
        {
            unitInfoModel.units?.Clear();
            Debug.Log("UnitInfoModel의 리스트를 초기화했습니다.");
        }
        else if (selectedScriptableObject is UnitGradeScriptableObject unitGrade)
        {
            unitGrade.gradeInfoList?.Clear();
            Debug.Log("unitGrade의 리스트를 초기화했습니다.");
        }
        else if(selectedScriptableObject is EnemyInfoScriptableObject enemyInfo)
        {
            enemyInfo.units?.Clear();
            Debug.Log("enemyInfo의 리스트를 초기화했습니다.");
        }
        else if(selectedScriptableObject is LoadingMessageScriptableObject loadingMessage)
        {
            loadingMessage.messages?.Clear();
            Debug.Log("loadingMessage의 리스트를 초기화했습니다.");
        }
        else
        {
            Debug.LogWarning("지원하지 않는 ScriptableObject 타입입니다.");
        }

        // ScriptableObject 변경 사항 저장
        EditorUtility.SetDirty(selectedScriptableObject);
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
            key = rowData[0],             // 첫 번째 열은 key
            size = int.Parse(rowData[1])
        };

        // List에 추가
        poolPreset.poolPresetInfoList.Add(presetInfo);
        Debug.Log($"AddressableAssetInfo 추가됨: {presetInfo.key}");

        // ScriptableObject 저장
        EditorUtility.SetDirty(poolPreset); // 변경 사항 저장
    }

    // CSV 데이터를 EnemySpawnRuleScriptableObject로 변환하여 List에 저장
    private void ConvertToSpawnRuleSO(string[] rowData)
    {
        // 선택한 ScriptableObject를 EnemySpawnRuleScriptableObject로 캐스팅
        var asset = selectedScriptableObject as SpawnRuleScriptableObject;

        if (asset == null)
        {
            Debug.LogError("EnemySpawnRuleScriptableObject로 캐스팅할 수 없습니다.");
            return;
        }


        // 스테이지 ID를 가져옴 (첫 번째 열)
        // int stageId = int.Parse(rowData[0]);

        // 새로운 EnemySpawnRule을 생성하여 데이터를 추가
        SpawnRuleScriptableObject.SpawnRule spawnRule = new SpawnRuleScriptableObject.SpawnRule
        {
            stageId = int.Parse(rowData[0]),
            key = rowData[1],             // 유닛 태그 (두 번째 열)
            spawnCount = int.Parse(rowData[2]),   // 스폰 횟수 (세 번째 열)
            minSpawnCount = int.Parse(rowData[3]),// 최소 스폰 횟수 (네 번째 열)
            maxSpawnCount = int.Parse(rowData[4]),// 최대 스폰 횟수 (다섯 번째 열)
            minInterval = float.Parse(rowData[5]),// 최소 스폰 간격 (여섯 번째 열)
            maxInterval = float.Parse(rowData[6]) // 최대 스폰 간격 (일곱 번째 열)
        };

        asset.spawnRules.Add(spawnRule);

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }

    private void ConvertToStageInfoSO(string[] rowData)
    {
        // 선택한 ScriptableObject를 SgateInfoScriptableObject 로 캐스팅
        var asset = selectedScriptableObject as StageInfoScriptableObject;

        if (asset == null)
        {
            Debug.LogError("SgateInfoScriptableObject 캐스팅할 수 없습니다.");
            return;
        }

        // 새로운 EnemySpawnRule을 생성하여 데이터를 추가
        StageInfoScriptableObject.StageInfo stageInfo = new StageInfoScriptableObject.StageInfo
        {
            stageId = int.Parse(rowData[0]),
            stageName = rowData[1],
            sceneName = rowData[2],
            description = rowData[3],
            timeLimit1 = int.Parse(rowData[4]),
            timeLimit2 = int.Parse(rowData[5]),
            AllyBossMaxHp = int.Parse(rowData[6]),
            EnemyBossMaxHp = int.Parse(rowData[7])
        };

        asset.stageInfoList.Add(stageInfo);

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }

    private void ConvertToUnitInfoModel(string[] rowData)
    {
        // 선택한 ScriptableObject를 SgateInfoScriptableObject 로 캐스팅
        var asset = selectedScriptableObject as AllyInfoScriptableObject;

        if (asset == null)
        {
            Debug.LogError("SgateInfoScriptableObject 캐스팅할 수 없습니다.");
            return;
        }

        // 새로운 EnemySpawnRule을 생성하여 데이터를 추가
        AllyInfoScriptableObject.UnitInfo unitInfo = new AllyInfoScriptableObject.UnitInfo
        {
            unitKey = rowData[0],
            unitName = rowData[1],
            description = rowData[2],
            cost = int.Parse(rowData[3])
        };

        asset.units.Add(unitInfo);

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }

    private void ConvertToUnitGrade(string[] rowData)
    {
        // 선택한 ScriptableObject를 UnitGradeSciptableObject 로 캐스팅
        var asset = selectedScriptableObject as UnitGradeScriptableObject;

        if (asset == null)
        {
            Debug.LogError("SgateInfoScriptableObject 캐스팅할 수 없습니다.");
            return;
        }

        // 새로운 EnemySpawnRule을 생성하여 데이터를 추가
        UnitGradeScriptableObject.GradeInfo gradeInfo = new UnitGradeScriptableObject.GradeInfo
        {
            unitKey = rowData[0],
            grade = int.Parse(rowData[1]),
            attack = float.Parse(rowData[2]),
            speed = float.Parse(rowData[3]),
            health = float.Parse(rowData[4]),
            special = float.Parse(rowData[5]),
            upgradeGold = int.Parse(rowData[6])
        };

        asset.gradeInfoList.Add(gradeInfo);

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }

    private void ConvertToEnemyInfo(string[] rowData)
    {
        // 선택한 ScriptableObject를 UnitGradeSciptableObject 로 캐스팅
        var asset = selectedScriptableObject as EnemyInfoScriptableObject;

        if (asset == null)
        {
            Debug.LogError("SgateInfoScriptableObject 캐스팅할 수 없습니다.");
            return;
        }

        // 새로운 EnemySpawnRule을 생성하여 데이터를 추가
        EnemyInfoScriptableObject.UnitInfo unitInfo = new EnemyInfoScriptableObject.UnitInfo
        {
            unitKey = rowData[0],
            attack = float.Parse(rowData[1]),
            speed = float.Parse(rowData[2]),
            health = float.Parse(rowData[3]),
            special = float.Parse(rowData[4]),
            gold = int.Parse(rowData[5])
        };

        asset.units.Add(unitInfo);

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }

    private void ConvertToLoadingMessage(string[] rowData)
    {
        // 선택한 ScriptableObject를 UnitGradeSciptableObject 로 캐스팅
        var asset = selectedScriptableObject as LoadingMessageScriptableObject;

        if (asset == null)
        {
            Debug.LogError("SgateInfoScriptableObject 캐스팅할 수 없습니다.");
            return;
        }

        asset.messages.Add(rowData[0]);

        // ScriptableObject 저장
        EditorUtility.SetDirty(asset); // 변경 사항 저장
    }
}