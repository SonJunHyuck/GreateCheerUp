using UnityEditor;
using System.IO;
using UnityEngine;

public class SceneEnumGenerator : MonoBehaviour
{
    private const string enumFilePath = "Assets/Scripts/0.Common/Scene/SceneKey.cs";

    [MenuItem("Tools/Generate Scene Enum")]
    public static void GenerateSceneEnum()
    {
        var scenes = EditorBuildSettings.scenes;
        string enumContent = "public enum SceneKey\n{\n";

        foreach (var scene in scenes)
        {
            if (scene.enabled)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                enumContent += $"    {sceneName},\n";
            }
        }
        enumContent += "}";

        File.WriteAllText(enumFilePath, enumContent);
        AssetDatabase.Refresh();
        Debug.Log($"Scene Enum 생성 완료: {enumFilePath}");
    }
}