using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private GameObject logoUI;
    [SerializeField] private GameObject progressBar;

    private void Start()
    {
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        logoUI.SetActive(true);
        progressBar.SetActive(false);

        yield return new WaitForSeconds(2f); // 로고 애니메이션 시간

        logoUI.SetActive(false);
        progressBar.SetActive(true);

        yield return AddressableUpdater.Instance.CheckAndDownloadAddressables();

        SceneManager.LoadScene("Title");
    }
}