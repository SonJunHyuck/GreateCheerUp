using Unity.Collections;
using UnityEngine;

public class ActorMovement : MonoBehaviour, IMovable
{
    public event System.Action<bool> OnStateChanged;

    public float moveSpeed = 5f;
    private Vector3 currentPosition;

    // 이동을 처리하는 메서드 (IMovable 인터페이스 구현)
    public void Move(float direction)
    {
        OnStateChanged?.Invoke(true);

        currentPosition = transform.position; 
        currentPosition.x += direction * moveSpeed * Time.deltaTime;
        currentPosition.x = Mathf.Clamp(currentPosition.x, -15.0f, 1.0f);
        transform.position = currentPosition;
    }
}
