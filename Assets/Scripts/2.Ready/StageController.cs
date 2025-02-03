
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour 
{
    // View에 의존성이 발생하지만, 더 명료함
    [SerializeField] private StageView stageView;

    private void OnEnable()
    {
        // 이벤트 구독
        Observer.Instance.OnRequestStageSelect += ResponseStageSelect;
        Observer.Instance.OnRequestSwitchPage += ResponseSwitchPage;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        Observer.Instance.OnRequestStageSelect -= ResponseStageSelect;
        Observer.Instance.OnRequestSwitchPage -= ResponseSwitchPage;
    }

    private void ResponseStageSelect(int stageId, string sceneName)
    {
        Debug.Log($"Selected Stage ID: {stageId}");

        // 선택된 StageId로 게임 씬 로드
        DataManager.Instance.currentStageId = stageId;

        // SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        // LoadSceneManager.Instance.LoadScene(sceneName);
        SceneController.Instance.LoadScene(sceneName);
    }

    private void ResponseSwitchPage(int increase)
    {
        stageView.UpdatePage(increase);
    }
}
