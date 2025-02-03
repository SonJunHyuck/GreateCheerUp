using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraAspectRatioHandler : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // 목표 화면 비율 (16:9)
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SetAspectRatio();
    }

    void SetAspectRatio()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // 세로가 더 긴 경우 - 위아래에 블랙 바 추가
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        else
        {
            // 가로가 더 긴 경우 - 좌우에 블랙 바 추가
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCamera.rect = rect;
        }
    }
}