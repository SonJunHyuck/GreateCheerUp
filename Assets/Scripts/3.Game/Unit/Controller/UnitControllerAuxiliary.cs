using UnityEngine;

public partial class UnitController
{
    // 방향
    public bool isFacingRight; // 기본적으로 왼쪽을 바라봄
    public bool isDesireRight;
    public int MovementDirection
    {
        get { return isDesireRight ? 1 : -1; }
    }

    // 이동 및 공격 방향 설정
    public void SetDirection()
    {
        if (MovementDirection == 1 && !isFacingRight)
        {
            // 오른쪽으로 진행, 왼쪽을 보고있음 -> Flip
            Flip();
        }
        else if (MovementDirection == -1 && isFacingRight)
        {
            // 왼쪽으로 진행, 오른쪽을 보고있음 -> Flip
            Flip();
        }
    }

    // 이미지 반전
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1); // 캐릭터 좌우 반전

        // 기본 왼쪽 보고 있음 
    }

    private void SetYParallelEffect()
    {
        Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y * 0.01f);
        transform.localPosition = pos;
    }
}
