using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenameObjectsTool : EditorWindow
{
    private NameListScriptableObject nameListSO; // ScriptableObject를 참조
    private List<GameObject> selectedObjects = new List<GameObject>(); // 선택된 오브젝트들

    [MenuItem("Tools/Rename Objects")]
    public static void ShowWindow()
    {
        GetWindow<RenameObjectsTool>("Rename Objects Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename Objects in Hierarchy", EditorStyles.boldLabel);

        // ScriptableObject 드래그 앤 드롭
        nameListSO = (NameListScriptableObject)EditorGUILayout.ObjectField(
            "Name List SO", nameListSO, typeof(NameListScriptableObject), false);

        // 하이어라키에서 선택된 오브젝트들 표시
        if (GUILayout.Button("Load Selected Objects"))
        {
            LoadSelectedObjects();
        }

        GUILayout.Label("Selected Objects:", EditorStyles.label);
        if (selectedObjects.Count > 0)
        {
            foreach (var obj in selectedObjects)
            {
                GUILayout.Label(obj.name);
            }
        }
        else
        {
            GUILayout.Label("No objects selected.");
        }

        // 이름 변경 버튼
        if (GUILayout.Button("Rename Selected Objects"))
        {
            RenameSelectedObjects();
        }
    }

    /// <summary>
    /// 하이어라키에서 선택된 오브젝트들을 로드합니다.
    /// </summary>
    private void LoadSelectedObjects()
    {
        selectedObjects.Clear();

        foreach (var obj in Selection.gameObjects)
        {
            selectedObjects.Add(obj);
        }

        Debug.Log($"Loaded {selectedObjects.Count} objects from hierarchy.");
    }

    /// <summary>
    /// ScriptableObject의 이름 리스트에 따라 선택된 오브젝트들의 이름을 변경합니다.
    /// </summary>
    private void RenameSelectedObjects()
    {
        if (nameListSO == null)
        {
            Debug.LogError("NameListScriptableObject is not assigned!");
            return;
        }

        if (selectedObjects.Count == 0)
        {
            Debug.LogWarning("No objects selected for renaming.");
            return;
        }

        List<string> nameList = nameListSO.names;

        if (nameList.Count < selectedObjects.Count)
        {
            Debug.LogWarning("Name list does not have enough names for all selected objects.");
        }

        for (int i = 0; i < selectedObjects.Count; i++)
        {
            if (i >= nameList.Count) break;

            string newName = nameList[i];
            GameObject obj = selectedObjects[i];
            Undo.RecordObject(obj, "Rename Object");
            obj.name = newName;
        }

        Debug.Log("Renamed objects successfully!");
    }
}