using UnityEditor;
using UnityEngine;

public class UnitUpgradeDebugEditorWindow : DebugDataEditorWindow<UnitUpgradeCollection>
{
    [MenuItem("Tools/Player Data Modifier/Unit Upgrade Debug Editor")]
    public static void ShowEditor()
    {
        var window = GetWindow<UnitUpgradeDebugEditorWindow>("Unit Upgrade Debug Editor");
        if (window == null)
        {
            Debug.LogError("Failed to create UnitUpgradeDebugEditorWindow.");
            return;
        }

        window.Show();
    }
}

public class CurrencyDebugEditorWindow : DebugDataEditorWindow<CurrencyData>
{
    [MenuItem("Tools/Player Data Modifier/Currency Debug Editor")]
    public static void ShowEditor()
    {
        var window = GetWindow<CurrencyDebugEditorWindow>("Currency Debug Editor");
        if (window == null)
        {
            Debug.LogError("Failed to create CurrencyDebugEditorWindow.");
            return;
        }

        window.Show();
    }
}

public class StageProgressDebugEditorWindow : DebugDataEditorWindow<StageProgressCollection>
{
    [MenuItem("Tools/Player Data Modifier/Stage Progress Debug Editor")]
    public static void ShowEditor()
    {
        var window = GetWindow<StageProgressDebugEditorWindow>("Stage Progress Debug Editor");
        if (window == null)
        {
            Debug.LogError("Failed to create StageProgressDebugEditorWindow.");
            return;
        }

        window.Show();
    }
}