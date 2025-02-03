using System;

public partial class Observer
{
    #region Panel Switch
    public event Action<string> OnRequestPanelSwitch;

    public void RequestPanelSwitch(string panelName)
    {
        OnRequestPanelSwitch?.Invoke(panelName);
    }
    #endregion

    #region UnitInfo
    public event Action<string, bool> OnRequestUnitInfo;
    public void RequestUnitInfoEvent(string unitKey)
    {
        OnRequestUnitInfo?.Invoke(unitKey, false);
    }

    public event Action OnRequestUpgradeUnit;
    public void RequestUpgradeUnit()
    {
        OnRequestUpgradeUnit?.Invoke();
    }
    #endregion

    #region Stage
    public event Action<int, string> OnRequestStageSelect;

    public void RequestStageSelect(int stageId, string sceneName)
    {
        OnRequestStageSelect?.Invoke(stageId, sceneName);
    }

    public event Action<int> OnRequestSwitchPage;
    public void RequestSwitchPage(int increase)
    {
        OnRequestSwitchPage?.Invoke(increase);
    }
    #endregion

    #region Notice
    public event Action<NoticeMessageView.MessageType> OnRequestNoticeMessage;
    public void RequestNoticeMessage(NoticeMessageView.MessageType messageType)
    {
        OnRequestNoticeMessage?.Invoke(messageType);
    }

    #endregion
}