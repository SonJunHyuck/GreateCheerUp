using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NoticeMessageView : MonoBehaviour
{
    [System.Serializable]
    public struct EnumToString
    {
        public MessageType messageType; // Enum
        public string message; // 연결된 오브젝트
    }

    public enum MessageType
    {
        NeedMoreGold,
        SuccessUpgrade
    }

    [SerializeField] private List<EnumToString> messageMappings;

    private Dictionary<MessageType, string> messageDictionary;

    [SerializeField] private GameObject objectText;
    [SerializeField] private Transform canvasTransform;

    private void Awake()
    {
        // Enum -> GameObject 매핑
        messageDictionary = new Dictionary<MessageType, string>();

        foreach (var mapping in messageMappings)
        {
            if (!messageDictionary.ContainsKey(mapping.messageType))
            {
                messageDictionary[mapping.messageType] = mapping.message;
            }
            else
            {
                Debug.LogWarning($"Duplicate UIObjectType found: {mapping.messageType}. Only the first entry will be used.");
            }
        }
    }

    private void OnEnable()
    {
        Observer.Instance.OnRequestNoticeMessage += ResponseNotice;
    }

    private void OnDisable()
    {
        Observer.Instance.OnRequestNoticeMessage -= ResponseNotice;
    }

    private void ResponseNotice(MessageType messageType)
    {
        // 요청된 오브젝트 활성화
        if (messageDictionary.TryGetValue(messageType, out var targetObject))
        {
            SpawnMessage(messageType);
        }
        else
        {
            Debug.LogWarning($"No object found for UIObjectType: {messageType}");
        }
    }

    public void SpawnMessage(MessageType messageType)
    {
        // 메시지 생성
        GameObject message = Instantiate(objectText, canvasTransform);
        
        // 텍스트 설정 (Assumes Text or TextMeshProUGUI component)
        Text textComponent = message.GetComponent<Text>();

        if (textComponent != null)
        {
            textComponent.text = messageDictionary[messageType];
        }
    }
}