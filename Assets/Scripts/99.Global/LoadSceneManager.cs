using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }

    [SerializeField] private GameObject loadingScreen; // 로딩 화면 오브젝트
    [SerializeField] public Slider progressBar;       // 프로그레스 바 UI
    [SerializeField] public Text textExplain;
    
    [SerializeField] private List<string> explains = new ();
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 모든 씬에서 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<CanvasScaler>().enabled = true;

        textExplain.text = explains[Random.Range(0, explains.Count)];

        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
        GetComponent<Canvas>().enabled = false;
        GetComponent<CanvasScaler>().enabled = false;
    }
}
