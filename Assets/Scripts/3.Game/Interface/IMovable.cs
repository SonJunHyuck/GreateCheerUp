// 기본적인 이동 인터페이스 (입력 필요 없음)
public interface IMovable
{
    event System.Action<bool> OnStateChanged;

    void Move(float direction);
}