using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : MonoBehaviour
{
    // UI에서 호출되는 메서드: 이동 입력값 설정 (버튼으로 입력값 전달)
    public void SetMoveInput(float input)
    {
        moveInput = input;
    }
}
