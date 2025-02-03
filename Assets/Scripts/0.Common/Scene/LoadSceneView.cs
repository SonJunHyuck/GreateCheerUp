using UnityEngine;
using UnityEngine.UI;

public class LoadSceneView : MonoBehaviour
{
    [SerializeField] private Slider progressBar;  // ProgressBar
    [SerializeField] private Text explainText;    // ExplainText

    /// <summary>
    /// ProgressBar를 업데이트합니다.
    /// </summary>
    /// <param name="progress">로딩 진행도 (0.0 ~ 1.0)</param>
    public void UpdateProgressBar(float progress)
    {
        if (progressBar != null)
            progressBar.value = progress;
    }

    /// <summary>
    /// ExplainText를 업데이트합니다.
    /// </summary>
    /// <param name="message">로딩 화면에 표시할 메시지</param>
    public void UpdateExplainText(string message)
    {
        if (explainText != null)
            explainText.text = message;
    }
}