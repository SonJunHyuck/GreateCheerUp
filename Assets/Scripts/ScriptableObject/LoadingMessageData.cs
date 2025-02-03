using UnityEngine;

[CreateAssetMenu(fileName = "LoadingMessageScriptableObject", menuName = "ScriptableObjects/LoadingMessageScriptableObject", order = 1)]
public class LoadingMessageScriptableObject : ScriptableObject
{
    [TextArea] // 다중 라인 입력 지원
    public System.Collections.Generic.List<string> messages; // 로딩 메시지 배열
}