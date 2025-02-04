using UnityEngine;
using UnityEditor;
using System.IO;

public class CanvasCaptureTool : EditorWindow
{
    private Canvas canvas; // 드래그 앤 드롭할 Canvas
    private string savePath = "CanvasCapture.png"; // 기본 파일명

    [MenuItem("Tools/Canvas Capture Tool")]
    public static void ShowWindow()
    {
        GetWindow<CanvasCaptureTool>("Canvas Capture");
    }

    void OnGUI()
    {
        GUILayout.Label("Canvas Capture Tool", EditorStyles.boldLabel);

        // Canvas 드래그 앤 드롭 필드
        canvas = (Canvas)EditorGUILayout.ObjectField("Target Canvas", canvas, typeof(Canvas), true);

        // 파일 경로 입력 필드
        savePath = EditorGUILayout.TextField("Save File Name", savePath);

        // 캡처 버튼
        if (GUILayout.Button("Save as PNG"))
        {
            if (canvas != null)
            {
                CaptureCanvasToPNG(canvas, savePath);
            }
            else
            {
                Debug.LogError("Canvas가 설정되지 않았습니다!");
            }
        }
    }

    private void CaptureCanvasToPNG(Canvas canvas, string fileName)
    {
        // Canvas 크기 가져오기
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        int width = (int)canvasRect.rect.width;
        int height = (int)canvasRect.rect.height;

        // RenderTexture 생성
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Camera uiCamera = canvas.worldCamera;

        // 기존 카메라 설정 저장
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // UI를 렌더링
        uiCamera.targetTexture = renderTexture;
        uiCamera.Render();
        uiCamera.targetTexture = null;

        // Texture2D로 변환
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        // 저장 경로 선택
        string path = EditorUtility.SaveFilePanel("Save Canvas as PNG", "", fileName, "png");
        if (string.IsNullOrEmpty(path)) return;

        // PNG로 저장
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        Debug.Log($"Canvas 캡처 저장 완료: {path}");

        // 리소스 정리
        RenderTexture.active = currentRT;
        DestroyImmediate(renderTexture);
        DestroyImmediate(texture);
    }
}