using UnityEditor;
using UnityEngine;
using System;
using System.IO;

public class DebugDataEditorWindow<T> : EditorWindow where T : class, new()
{
    private string filePath = "Assets/Data/player_data.json";
    private string decryptedData;
    private bool dataIntegrityValid = false;

    public static void ShowWindow()
    {
        // 이미 열린 창이 있는지 확인
        var existingWindow = Resources.FindObjectsOfTypeAll<DebugDataEditorWindow<T>>();
        if (existingWindow.Length > 0)
        {
            existingWindow[0].Focus(); // 기존 창으로 포커스 이동
            return;
        }

        // 새 창 생성
        DebugDataEditorWindow<T> window = GetWindow<DebugDataEditorWindow<T>>();
        if (window == null)
        {
            Debug.LogError("Failed to create DebugDataEditorWindow.");
            return;
        }

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label($"Debug Data Editor for {typeof(T).Name}", EditorStyles.boldLabel);

        filePath = EditorGUILayout.TextField("File Path", filePath);

        if (GUILayout.Button("Load and Decrypt"))
        {
            LoadAndDecrypt();
        }

        if (!string.IsNullOrEmpty(decryptedData))
        {
            EditorGUILayout.Space();
            GUILayout.Label($"Integrity Status: {(dataIntegrityValid ? "Valid" : "Invalid")}", 
                            new GUIStyle { normal = { textColor = dataIntegrityValid ? Color.green : Color.red } });

            decryptedData = EditorGUILayout.TextArea(decryptedData, GUILayout.Height(200));

            if (GUILayout.Button("Save and Encrypt"))
            {
                SaveAndEncrypt();
            }
        }
    }

    private void LoadAndDecrypt()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found.");
            return;
        }

        try
        {
            var data = DataEncryptionUtility.LoadEncryptedDataWithIntegrity<T>(filePath);

            if (data == null)
            {
                decryptedData = string.Empty;
                dataIntegrityValid = false;
            }
            else
            {
                decryptedData = JsonUtility.ToJson(data, true);
                dataIntegrityValid = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load data: {ex.Message}");
            decryptedData = string.Empty;
            dataIntegrityValid = false;
        }
    }

    private void SaveAndEncrypt()
    {
        if (string.IsNullOrEmpty(decryptedData))
        {
            Debug.LogError("No data to encrypt.");
            return;
        }

        try
        {
            var parsedData = JsonUtility.FromJson<T>(decryptedData);
            DataEncryptionUtility.SaveEncryptedDataWithIntegrity(filePath, parsedData);
            Debug.Log("Data saved successfully with encryption and integrity.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save data: {ex.Message}");
        }
    }
}