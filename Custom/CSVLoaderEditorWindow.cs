using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class CSVLoaderEditorWindow : EditorWindow
{
    private string csvFilePath = ""; // CSV 파일 경로
    private Type selectedType;       // ScriptableObject 타입 선택
    private string[] availableTypes; // 사용할 수 있는 ScriptableObject 타입 목록
    private string[] filteredTypes;  // 검색된 ScriptableObject 타입 목록
    private int selectedTypeIndex = 0;

    private string searchQuery = ""; // 검색어
    private bool searchCompleted = false; // 검색 완료 여부

    [MenuItem("Tools/CSV Loader")]
    public static void ShowWindow()
    {
        GetWindow<CSVLoaderEditorWindow>("CSV Loader");
    }

    private void OnEnable()
    {
        // ScriptableObject 타입을 자동으로 검색하여 사용할 수 있는 타입 목록을 만듦
        FindAvailableScriptableObjectTypes();
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV Loader", EditorStyles.boldLabel);

        // CSV 파일 경로 입력
        GUILayout.Label("CSV File Path:");
        csvFilePath = EditorGUILayout.TextField(csvFilePath);

        // 검색 필드 추가
        GUILayout.Label("Search ScriptableObject Type:");
        searchQuery = EditorGUILayout.TextField(searchQuery);

        // 검색 버튼
        if (GUILayout.Button("Search"))
        {
            SearchScriptableObjectTypes();
        }


        // 검색 완료 후에만 드롭다운을 표시
        if (searchCompleted)
        {
            // ScriptableObject 타입 선택 드롭다운
            GUILayout.Label("Select ScriptableObject Type:");
            selectedTypeIndex = EditorGUILayout.Popup(selectedTypeIndex, filteredTypes);

            // 선택된 타입의 풀네임을 사용하여 타입을 가져옴
            if (filteredTypes.Length > 0 && selectedTypeIndex >= 0 && selectedTypeIndex < filteredTypes.Length)
            {
                selectedType = Type.GetType(filteredTypes[selectedTypeIndex]);
            }
        }

        // CSV 파일 로드 버튼
        if (GUILayout.Button("Load CSV and Create ScriptableObjects"))
        {
            LoadCSVAndCreateScriptableObjects();
        }
    }

    // ScriptableObject 타입을 검색하는 기능
    private void SearchScriptableObjectTypes()
    {
        // 검색어에 맞는 타입 목록을 필터링
        filteredTypes = FilterAvailableTypes(searchQuery);

        // 드롭다운 인덱스 초기화 및 검색 완료 플래그 설정
        selectedTypeIndex = 0;
        searchCompleted = true;
    }

    // ScriptableObject 타입을 검색하는 기능
    private string[] FilterAvailableTypes(string query)
    {
        // 검색어가 없으면 전체 목록 반환
        if (string.IsNullOrEmpty(query))
        {
            return availableTypes;
        }

        // 검색어에 맞는 타입 목록을 필터링하여 반환 (대소문자 구분 없이 검색)
        return availableTypes.Where(type => type.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
    }

    private void FindAvailableScriptableObjectTypes()
    {
        // 모든 ScriptableObject 타입을 찾아 목록에 저장
        var scriptableObjectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsAbstract)
            .ToList();

        // ScriptableObject 타입 이름을 배열로 저장
        availableTypes = scriptableObjectTypes.Select(type => type.FullName).ToArray();
    }

    private void LoadCSVAndCreateScriptableObjects()
    {
        if (string.IsNullOrEmpty(csvFilePath))
        {
            Debug.LogError("CSV 파일 경로가 설정되지 않았습니다.");
            return;
        }

        if (!System.IO.File.Exists(csvFilePath))
        {
            Debug.LogError("CSV 파일이 존재하지 않습니다.");
            return;
        }

        if (selectedType == null)
        {
            Debug.LogError("ScriptableObject 타입이 선택되지 않았습니다.");
            return;
        }

        // CSVLoader 인스턴스 생성 및 로드 메서드 호출
        CSVLoader csvLoader = new CSVLoader
        {
            csvFilePath = csvFilePath
        };

        // 제네릭 메서드를 런타임에 동적으로 호출
        var method = typeof(CSVLoader).GetMethod("LoadCSVAndCreateScriptableObjects");
        var genericMethod = method.MakeGenericMethod(selectedType);
        genericMethod.Invoke(csvLoader, null);
    }
}