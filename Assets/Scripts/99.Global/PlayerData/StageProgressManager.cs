using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class StageProgressManager
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/stage_progress.json";
    public static StageProgressCollection progressData;

    static StageProgressManager()
    {
        LoadProgress();
    }

    // 스테이지 클리어 저장
    public static void SaveStageClear(int stageId, int clearGrade)
    {
        var stage = progressData.stages.Find(s => s.id == stageId);
        if (stage == null)
        {
            stage = new StageProgress { id = stageId, clearGrade = clearGrade };
            progressData.stages.Add(stage);
        }
        else
        {
            if(stage.clearGrade < clearGrade)
            {
                stage.clearGrade = clearGrade;
            }
        }

        SaveProgress();
    }

    private static void SaveProgress()
    {
        DataEncryptionUtility.SaveEncryptedDataWithIntegrity(SavePath, progressData);
    }

    private static void LoadProgress()
    {
        var data = DataEncryptionUtility.LoadEncryptedDataWithIntegrity<StageProgressCollection>(SavePath);

        if(data == null)
        {
            StageProgress progress = new StageProgress();
            progress.id = 1;
            progress.clearGrade = 0;

            data = new StageProgressCollection();
            data.stages.Add(progress);
            progressData = data;
            SaveProgress();
            
            return;
        }
        else
        {
            progressData = data;
        }
    }

    // 스테이지 클리어 여부 확인
    public static int GetStageClearGrade(int stageId)
    {
        var stage = progressData.stages.Find(s => s.id == stageId);

        return stage != null ? stage.clearGrade : 0;
    }

    // 이전 스테이지가 클리어되면 오픈
    public static bool IsOpenStage(int stageId)
    {
        if(stageId == 1)
        {
            return true;
        }

        var stage = progressData.stages.Find(s => s.id == stageId - 1);

        return stage != null && stage.clearGrade > 0;
    }

    // 진행 상태 로드
    // private static void LoadProgress()
    // {
    //     if (File.Exists(SavePath))
    //     {
    //         var json = File.ReadAllText(SavePath);
    //         progressData = JsonUtility.FromJson<StageProgressCollection>(json);
    //         Debug.Log("Progress loaded successfully.");
    //     }
    //     else
    //     {
    //         progressData = new StageProgressCollection(); // 초기화
    //         Debug.LogWarning("No progress file found. Starting with new data.");
    //     }
    // }
}

[System.Serializable]
public class StageProgress
{
    public int id;
    public int clearGrade;
}

[System.Serializable]
public class StageProgressCollection
{
    public List<StageProgress> stages = new List<StageProgress>();
}