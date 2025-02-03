using System;

public partial class Observer
{
    #region SpawnAlly
    public delegate bool SpawnConsumeEnergy(int cost);
    public event SpawnConsumeEnergy onRequestConsumeEnergy;

    public delegate bool SpawnAllyDelegate(string key);
    public event SpawnAllyDelegate onRequestSpawnAlly;

    public void RequestSpawnAlly(string key, int cost)
    {
        if((bool)onRequestConsumeEnergy?.Invoke(cost))
        {
            onRequestSpawnAlly?.Invoke(key);
        }
    }
    #endregion

    #region MovePlayer
    public event Action<float> onRequestMovePlayer;

    public void RequestMovePlayer(float inputDir)
    {
        onRequestMovePlayer?.Invoke(inputDir);
    }
    #endregion

    #region Result Stage
    public event Action<bool, int, int, int> onStageResultPopup;

    public void RaiseStageResultPopup(bool isClear, int clearTime, int gainGold, int clearGrade)
    {
        onStageResultPopup?.Invoke(isClear, clearTime, gainGold, clearGrade);
    }
    #endregion
}