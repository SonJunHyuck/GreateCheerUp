using UnityEditor;
using UnityEngine;

public class PrefabHierarchyModifierWindow : EditorWindow
{
    public GameObject[] prefabs; // 선택할 프리팹 리스트
    public GameObject objectToAttach; // 상위 혹은 하위로 붙일 오브젝트
    private bool attachAsChild = true; // 기본적으로 하위 오브젝트로 붙이는 옵션

    [MenuItem("Tools/Prefab Hierarchy Modifier")]
    public static void ShowWindow()
    {
        GetWindow<PrefabHierarchyModifierWindow>("Prefab Hierarchy Modifier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Hierarchy Modifier", EditorStyles.boldLabel);

        // 프리팹 선택 필드
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("1. Select Prefabs", EditorStyles.boldLabel);
        SerializedObject so = new SerializedObject(this);
        SerializedProperty prefabsProperty = so.FindProperty("prefabs");
        EditorGUILayout.PropertyField(prefabsProperty, true); // true를 통해 배열로 표시
        so.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();

        // 붙일 오브젝트 선택
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("2. Select Object to Attach", EditorStyles.boldLabel);
        objectToAttach = (GameObject)EditorGUILayout.ObjectField("Object to Attach", objectToAttach, typeof(GameObject), true);
        EditorGUILayout.EndVertical();

        // 상위 또는 하위 선택 옵션
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("3. Attach as Parent or Child", EditorStyles.boldLabel);
        attachAsChild = EditorGUILayout.Toggle("Attach as Child", attachAsChild);
        if (attachAsChild)
        {
            GUILayout.Label("Object will be attached as a child of the selected prefabs.");
        }
        else
        {
            GUILayout.Label("Object will be attached as a parent of the selected prefabs.");
        }
        EditorGUILayout.EndVertical();

        // 실행 버튼
        EditorGUILayout.Space();
        if (GUILayout.Button("Modify Prefabs"))
        {
            ModifyPrefabs();
        }
    }

    // 프리팹에 상위 혹은 하위 오브젝트를 추가하는 메서드
    private void ModifyPrefabs()
    {
        if (objectToAttach == null)
        {
            Debug.LogWarning("No object selected to attach.");
            return;
        }

        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null) continue;

            // 프리팹 인스턴스화
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (attachAsChild)
            {
                // 선택된 프리팹에 하위 오브젝트로 붙이기
                GameObject newChild = Instantiate(objectToAttach);
                newChild.transform.SetParent(instance.transform);
                newChild.name = prefab.name;
                Debug.Log($"Attached {objectToAttach.name} as a child of {instance.name}.");
            }
            else
            {
                // 선택된 프리팹에 상위 오브젝트로 붙이기
                GameObject newParent = Instantiate(objectToAttach);
                instance.transform.SetParent(newParent.transform);
                newParent.name = prefab.name;
                Debug.Log($"Attached {objectToAttach.name} as a parent of {instance.name}.");
            }

            // 프리팹에 변경 사항 적용
            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);

            // 인스턴스 삭제
            DestroyImmediate(instance);
        }

        Debug.Log("Prefab hierarchy modification complete.");
    }
}