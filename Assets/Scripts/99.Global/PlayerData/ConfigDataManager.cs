using System.IO;
using UnityEngine;

public static class ConfigDataManager
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/config_data.json";
    private static ConfigData configData;

    static ConfigDataManager()
    {
        LoadConfig();
    }

    public static float GetBgmVolume => configData.bgmVolume;
    public static void SetBgmVolume(float volume)
    {
        configData.bgmVolume = volume;
        SaveConfig();
    }

    public static float GetSfxVolume => configData.sfxVolume;
    public static void SetSfxVolume(float volume)
    {
        configData.sfxVolume = volume;
        SaveConfig();
    }

    private static void SaveConfig()
    {
        File.WriteAllText(SavePath, JsonUtility.ToJson(configData, true));
    }

    private static void LoadConfig()
    {
        if (File.Exists(SavePath))
        {
            var json = File.ReadAllText(SavePath);
            configData = JsonUtility.FromJson<ConfigData>(json);
            Debug.Log("Configuration Data loaded successfully.");
        }
        else
        {
            configData = new ConfigData(); // 초기화
            Debug.LogWarning("No progress file found. Starting with new data.");
        }
    }
}

[System.Serializable]
public class ConfigData
{
    public float bgmVolume;
    public float sfxVolume;

    public ConfigData()
    {
        bgmVolume = 1;
        sfxVolume = 1;
    }
}