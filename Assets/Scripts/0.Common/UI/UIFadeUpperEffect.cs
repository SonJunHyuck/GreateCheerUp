using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeUpperEffect : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 1f; // 애니메이션 지속 시간
    [SerializeField] private float yOffset = 20f;    // Y축 이동 거리

    private Vector2 originalPosition; // 원래 위치

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (rectTransform == null || canvasGroup == null)
        {
            Debug.LogError("UIFadeUpperEffect requires a RectTransform and CanvasGroup component.");
        }

        // 초기 상태 저장
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 1f; // 시작 시 alpha 초기화
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOutEffect());
    }

    private IEnumerator FadeOutEffect()
    {
        float elapsed = 0f;

        Vector2 startPosition = originalPosition;
        Vector2 targetPosition = originalPosition + new Vector2(0, yOffset);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            // Y축으로 이동
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            // Alpha 값 줄이기
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // 효과 종료 후 처리
        Destroy(gameObject);
    }
}