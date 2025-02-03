using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [SerializeField] private string loadingSceneName = "Loading";          // 로딩 씬 이름
    [SerializeField] private LoadingMessageScriptableObject loadingMessageData;       // ScriptableObject 참조

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 싱글턴 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SceneKey sceneKey)
    {
        string sceneName = sceneKey.ToString();
        StartCoroutine(LoadSceneWithView(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithView(sceneName));
    }

    private IEnumerator LoadSceneWithView(string sceneName)
    {
        // 1. 로딩 씬 로드
        yield return SceneManager.LoadSceneAsync(loadingSceneName);

        // 2. 로딩 UI 가져오기
        var loadSceneView = FindObjectOfType<LoadSceneView>();
        if (loadSceneView == null)
        {
            Debug.LogError("LoadSceneView를 찾을 수 없습니다.");
            yield break;
        }

        // 3. 로딩 텍스트 초기화
        if (loadingMessageData != null && loadingMessageData.messages.Count > 0)
        {
            // ScriptableObject에서 랜덤 메시지 가져오기
            string randomMessage = loadingMessageData.messages[Random.Range(0, loadingMessageData.messages.Count)];
            loadSceneView.UpdateExplainText(randomMessage);
        }
        else
        {
            loadSceneView.UpdateExplainText("Loading...");
        }

        // 4. 대상 Scene 비동기 로드
        AsyncOperation loadTargetScene = SceneManager.LoadSceneAsync(sceneName);
        loadTargetScene.allowSceneActivation = false;

        while (!loadTargetScene.isDone)
        {
            float progress = Mathf.Clamp01(loadTargetScene.progress / 0.9f);
            loadSceneView.UpdateProgressBar(progress);

            if (loadTargetScene.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                loadTargetScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}