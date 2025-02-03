using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class PrefabComponentCopier : EditorWindow
{
    private GameObject sourcePrefab; // 프리팹 A (소스)
    private List<GameObject> targetPrefabs = new List<GameObject>(); // 타겟 프리팹 리스트

    [MenuItem("Tools/Prefab Component Copier")]
    public static void ShowWindow()
    {
        GetWindow<PrefabComponentCopier>("Prefab Component Copier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy Components from Source to Target Prefabs", EditorStyles.boldLabel);

        // 소스 프리팹 필드
        sourcePrefab = (GameObject)EditorGUILayout.ObjectField("Source Prefab (A)", sourcePrefab, typeof(GameObject), true);

        GUILayout.Space(10);

        // 타겟 프리팹 리스트
        GUILayout.Label("Target Prefabs (B)", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Target Prefab Slot"))
        {
            targetPrefabs.Add(null);
        }

        // 타겟 프리팹 슬롯 생성
        for (int i = 0; i < targetPrefabs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            targetPrefabs[i] = (GameObject)EditorGUILayout.ObjectField($"Target Prefab {i + 1}", targetPrefabs[i], typeof(GameObject), true);

            // 삭제 버튼
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                targetPrefabs.RemoveAt(i);
                i--; // 리스트 재정렬
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // 컴포넌트 복사 버튼
        if (GUILayout.Button("Copy Components"))
        {
            if (sourcePrefab == null || targetPrefabs.Count == 0)
            {
                Debug.LogError("Source Prefab and at least one Target Prefab must be assigned.");
                return;
            }

            CopyComponentsToTargets();
        }
    }

    private void CopyComponentsToTargets()
    {
        foreach (var targetPrefab in targetPrefabs)
        {
            if (targetPrefab == null)
            {
                Debug.LogWarning("One of the target prefabs is null. Skipping.");
                continue;
            }

            CopyComponents(sourcePrefab, targetPrefab);
        }
    }

    private void CopyComponents(GameObject source, GameObject target)
    {
        Component[] sourceComponents = source.GetComponents<Component>();
        Component[] targetComponents = target.GetComponents<Component>();

        foreach (Component sourceComponent in sourceComponents)
        {
            // Transform은 기본 컴포넌트이므로 무시
            if (sourceComponent is Transform) continue;

            // 대상 프리팹에서 동일한 컴포넌트를 찾음
            Component targetComponent = target.GetComponent(sourceComponent.GetType());

            if (targetComponent == null)
            {
                // 대상에 컴포넌트가 없으면 추가
                UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponent);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(target);
                Debug.Log($"Copied new component: {sourceComponent.GetType().Name} to {target.name}");
            }
            else
            {
                // 대상에 동일한 컴포넌트가 있으면 값만 복사
                UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponent);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(targetComponent);
                Debug.Log($"Updated existing component: {sourceComponent.GetType().Name} on {target.name}");
            }
        }

        Debug.Log($"Completed copying components to {target.name}");
    }
}