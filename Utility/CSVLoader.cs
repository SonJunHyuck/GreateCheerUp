using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class CSVLoader : MonoBehaviour
{
    // CSV 파일 경로
    public string csvFilePath;

    // CSV 파일을 로드하고, ScriptableObject를 생성하는 제네릭 메서드
    public void LoadCSVAndCreateScriptableObjects<T>() where T : ScriptableObject
    {
        // CSV 파일을 읽어옵니다.
        string[] data = File.ReadAllLines(csvFilePath);

        // 첫 번째 줄은 헤더이므로 1부터 시작
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');

            // 제네릭 타입의 ScriptableObject를 생성
            T newObject = ScriptableObject.CreateInstance<T>();

            // CSV 파일의 헤더와 데이터를 매핑하여 ScriptableObject 필드에 값을 설정
            SetFieldsFromCSV(newObject, data[0].Split(','), row);

            // ScriptableObject를 에셋으로 저장
            string objectName = typeof(T).Name + "_" + i;
            AssetDatabase.CreateAsset(newObject, $"Assets/Data/{objectName}.asset");
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"{typeof(T).Name} 객체 생성 완료");
    }

    // CSV 데이터를 ScriptableObject의 필드에 매핑하는 메서드
    private void SetFieldsFromCSV<T>(T obj, string[] headers, string[] values) where T : ScriptableObject
    {
        Type type = obj.GetType();

        // CSV의 헤더와 ScriptableObject의 필드를 매핑
        for (int i = 0; i < headers.Length; i++)
        {
            FieldInfo field = type.GetField(headers[i], BindingFlags.Public | BindingFlags.Instance);

            if (field != null && i < values.Length)
            {
                object convertedValue = ConvertValue(field.FieldType, values[i]);
                field.SetValue(obj, convertedValue);
            }
        }
    }

    // CSV에서 읽어온 데이터를 필드 타입에 맞게 변환하는 메서드
    private object ConvertValue(Type fieldType, string value)
    {
        if (fieldType == typeof(int))
            return int.Parse(value);
        else if (fieldType == typeof(float))
            return float.Parse(value);
        else if (fieldType == typeof(bool))
            return bool.Parse(value);
        else if (fieldType == typeof(string))
            return value;

        return null;
    }
}