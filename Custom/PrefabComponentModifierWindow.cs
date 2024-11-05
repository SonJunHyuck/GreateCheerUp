using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class PrefabComponentModifierWindow : EditorWindow
{
    public GameObject[] prefabs; // 프리팹 리스트
    private int removeComponentIndex = 0; // 제거할 컴포넌트의 드롭다운 인덱스
    private int addComponentIndex = 0; // 추가할 컴포넌트의 드롭다운 인덱스

    // 필터링을 통해 제외할 기본 컴포넌트 타입
    private readonly List<Type> protectedComponents = new List<Type>
    {
        // typeof(Transform), // Transform은 보호 대상
        typeof(MonoBehaviour) // MonoBehaviour의 직접적 제거 방지
    };

    // 프로젝트 내 모든 컴포넌트를 저장하는 리스트
    private List<Type> allComponents;
    private List<Type> filteredComponents; // 필터링된 컴포넌트 리스트
    private string[] filteredComponentNames;

    // 선택된 컴포넌트 리스트 (제거 및 추가용)
    private List<Type> componentsToRemove = new List<Type>(); // 제거할 컴포넌트 리스트
    private List<Type> componentsToAdd = new List<Type>(); // 추가할 컴포넌트 리스트

    // 커스텀 필터링 규칙들을 저장할 리스트
    private List<Func<Type, bool>> customFilters = new List<Func<Type, bool>>();

    // UI에서 설정 가능한 필터 규칙 상태
    private bool filterByNamespace = false;
    private bool filterByName = false;
    private bool filterByInterface = false;
    private string namespaceFilter = "UnityEngine"; // 기본 네임스페이스 필터
    private string nameFilter = ""; // 이름 필터
    private string interfaceFilter = "IDisposable"; // 인터페이스 필터

    private Vector2 scrollPos; // 스크롤 위치 저장

    private void OnEnable()
    {
        // 프로젝트에서 사용 가능한 모든 컴포넌트 탐색
        allComponents = GetAllProjectComponents();

        // 기본 필터링 규칙 추가 (보호된 컴포넌트 제외)
        customFilters.Add(component => !protectedComponents.Contains(component));

        // 초기 필터링 적용
        ApplyFilters();
    }

    // 프로젝트 내 모든 MonoBehaviour와 Component 클래스를 탐색하는 메서드
    private List<Type> GetAllProjectComponents()
    {
        // 유니티 어셈블리 내의 모든 타입을 탐색하여 Component의 서브클래스를 포함
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> allComponents = new List<Type>();

        foreach (var assembly in assemblies)
        {
            try
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    // Component의 서브클래스인지 확인하고, 추상 클래스가 아닌지 확인
                    if (typeof(Component).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        allComponents.Add(type);
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                // 일부 어셈블리는 로드 실패 가능, 에러 로그 출력
                Debug.LogWarning($"Could not load types from assembly: {assembly.FullName}. Error: {ex.Message}");
            }
        }

        return allComponents;
    }

    [MenuItem("Tools/Prefab Modifier")]
    public static void ShowWindow()
    {
        GetWindow<PrefabComponentModifierWindow>("Prefab Modifier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Modifier", EditorStyles.boldLabel);

        // 스크롤 뷰 시작
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // 프리팹 배열 필드
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("1. Select Prefabs", EditorStyles.boldLabel);
        SerializedObject so = new SerializedObject(this);
        SerializedProperty prefabsProperty = so.FindProperty("prefabs");
        EditorGUILayout.PropertyField(prefabsProperty, true);
        so.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();

        // 필터 설정 UI
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("2. Filter Settings", EditorStyles.boldLabel);

        filterByNamespace = EditorGUILayout.Toggle("Filter by Namespace", filterByNamespace);
        if (filterByNamespace)
        {
            namespaceFilter = EditorGUILayout.TextField("Namespace Filter", namespaceFilter);
        }

        filterByName = EditorGUILayout.Toggle("Filter by Name", filterByName);
        if (filterByName)
        {
            nameFilter = EditorGUILayout.TextField("Name Filter", nameFilter);
        }

        filterByInterface = EditorGUILayout.Toggle("Filter by Interface", filterByInterface);
        if (filterByInterface)
        {
            interfaceFilter = EditorGUILayout.TextField("Interface Filter", interfaceFilter);
        }

        if (GUILayout.Button("Apply Filters"))
        {
            ApplyFilters();
        }
        EditorGUILayout.EndVertical();

        // 컴포넌트 선택 UI
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("3. Select Components to Remove", EditorStyles.boldLabel);

        if (filteredComponentNames != null && filteredComponentNames.Length > 0)
        {
            removeComponentIndex = EditorGUILayout.Popup(removeComponentIndex, filteredComponentNames);
            if (GUILayout.Button("RemoveComponent Select"))
            {
                Type selectedComponent = filteredComponents[removeComponentIndex];
                if (!componentsToRemove.Contains(selectedComponent))
                {
                    componentsToRemove.Add(selectedComponent);
                }
            }
        }
        else
        {
            GUILayout.Label("No components available with current filter.");
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("4. Select Components to Add", EditorStyles.boldLabel);

        if (filteredComponentNames != null && filteredComponentNames.Length > 0)
        {
            addComponentIndex = EditorGUILayout.Popup(addComponentIndex, filteredComponentNames);
            if (GUILayout.Button("AddComponent Select"))
            {
                Type selectedComponent = filteredComponents[addComponentIndex];
                if (!componentsToAdd.Contains(selectedComponent))
                {
                    componentsToAdd.Add(selectedComponent);
                }
            }
        }
        else
        {
            GUILayout.Label("No components available with current filter.");
        }
        EditorGUILayout.EndVertical();

        // 선택된 컴포넌트 리스트 표시
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("5. Selected Components", EditorStyles.boldLabel);
        
        GUILayout.Label("Components to Remove:", EditorStyles.boldLabel);
        GUIStyle redStyle = new GUIStyle(EditorStyles.label);
        redStyle.normal.textColor = Color.red;
        foreach (var component in componentsToRemove)
        {
            GUILayout.Label(component.Name, redStyle);
        }

        GUILayout.Label("Components to Add:", EditorStyles.boldLabel);
        GUIStyle greenStyle = new GUIStyle(EditorStyles.label);
        greenStyle.normal.textColor = Color.green;
        foreach (var component in componentsToAdd)
        {
            GUILayout.Label(component.Name, greenStyle);
        }
        EditorGUILayout.EndVertical();

        // 리셋 및 수정 버튼
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Reset"))
        {
            ResetSelections();
        }
        if (GUILayout.Button("Modify Prefabs"))
        {
            ModifyPrefabs();
        }
        EditorGUILayout.EndVertical();

        // 스크롤 뷰 끝
        EditorGUILayout.EndScrollView();
    }

    private void ReplaceRectTransformWithTransform(GameObject instance)
{
    // 1. RectTransform 강제 제거
    Component rectTransform = instance.GetComponent<RectTransform>();
    if (rectTransform != null)
    {
        DestroyImmediate(rectTransform, true);  // 하이어라키 상의 연결도 제거
        Debug.Log("RectTransform forcibly removed.");
    }

    // 2. Transform을 강제로 다시 추가
    if (instance.GetComponent<Transform>() == null)
    {
        instance.AddComponent<Transform>();
        Debug.Log("Transform added to the GameObject.");
    }
    else
    {
        Debug.LogWarning("Transform already exists on this GameObject.");
    }
}

    // 필터 규칙을 적용하여 필터링된 컴포넌트를 업데이트
    private void ApplyFilters()
    {
        customFilters.Clear();
        customFilters.Add(component => !protectedComponents.Contains(component)); // 기본 보호 컴포넌트 필터

        // 네임스페이스 필터 추가
        if (filterByNamespace)
        {
            customFilters.Add(component => component.Namespace != null && component.Namespace.Contains(namespaceFilter));
        }

        // 이름 필터 추가
        if (filterByName)
        {
            customFilters.Add(component => component.Name.Contains(nameFilter));
        }

        // 인터페이스 필터 추가
        if (filterByInterface)
        {
            customFilters.Add(component => component.GetInterfaces().Any(i => i.Name == interfaceFilter));
        }

        UpdateFilteredComponents();
    }

    // 필터링된 컴포넌트 리스트를 갱신하는 메서드
    private void UpdateFilteredComponents()
    {
        filteredComponents = allComponents
            .Where(component => customFilters.All(filter => filter(component)))
            .ToList();

        filteredComponentNames = filteredComponents.Select(c => c.Name).ToArray();

        // UI 갱신
        Repaint();
    }

    private void ModifyPrefabs()
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null) continue;

            // 프리팹을 임시로 인스턴스화
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            // RectTransform이 있는지 확인 후 Transform으로 교체
            if (instance.GetComponent<RectTransform>() != null)
            {
                ReplaceRectTransformWithTransform(instance);
            }

            // 나머지 컴포넌트 작업(제거 및 추가)
            foreach (Type componentToRemove in componentsToRemove)
            {
                RemoveComponent(instance, componentToRemove);
            }

            foreach (Type componentToAdd in componentsToAdd)
            {
                AddComponent(instance, componentToAdd);
            }

            // 프리팹에 변경 사항 적용
            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);

            // 인스턴스 삭제
            DestroyImmediate(instance);
        }

        Debug.Log("Prefab modification complete.");
    }

    // 컴포넌트 제거 메서드
    private void RemoveComponent(GameObject instance, Type componentType)
    {
        Component[] components = instance.GetComponentsInChildren(componentType, true);
        foreach (Component component in components)
        {
            if (!protectedComponents.Contains(component.GetType())) // 보호 컴포넌트는 제거하지 않음
            {
                DestroyImmediate(component);
            }
        }
    }

    // 컴포넌트 추가 메서드
    private void AddComponent(GameObject instance, Type componentType)
    {
        if (instance.GetComponent(componentType) == null)
        {
            instance.AddComponent(componentType);
        }
    }

    // 선택된 컴포넌트 리스트를 리셋하는 메서드
    private void ResetSelections()
    {
        componentsToRemove.Clear();
        componentsToAdd.Clear();
        Debug.Log("Selections have been reset.");
    }
}