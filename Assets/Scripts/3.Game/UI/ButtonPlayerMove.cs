using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonPlayerMove : MonoBehaviour
{
    public float dir; // Inspector에서 설정

    private void Awake()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

        // PointerClick 이벤트 등록
        AddEventTrigger(trigger, OnPointerDown, EventTriggerType.PointerDown);

        // PointerEnter 이벤트 등록
        AddEventTrigger(trigger, OnPointerUp, EventTriggerType.PointerUp);
    }

    // EventTrigger에 리스너 추가하는 헬퍼 메서드
    private void AddEventTrigger(EventTrigger trigger, System.Action<BaseEventData> action, EventTriggerType eventType)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => { action.Invoke(eventData); });
        trigger.triggers.Add(entry);
    }

     // PointerClick 이벤트 처리
    private void OnPointerDown(BaseEventData eventData)
    {
        Observer.Instance.RequestMovePlayer(dir);
    }

    // PointerEnter 이벤트 처리
    private void OnPointerUp(BaseEventData eventData)
    {
        Observer.Instance.RequestMovePlayer(0);
    }
}