using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : MonoBehaviour
{
    private Animator animator;

    private IMovable movable; // IInputMovable 인터페이스 참조
    private PlayerAttack playerAttack; // PlayerAttack 스크립트 참조

    private float moveInput;            // 입력 값을 저장

    public bool isFacingRight = true; // 기본 방향 (좌측)

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // IInputMovable 인터페이스를 구현한 컴포넌트를 가져옴
        movable = GetComponent<IMovable>();
        // PlayerAttack 스크립트 참조 가져오기
        playerAttack = GetComponent<PlayerAttack>();

        movable.OnStateChanged += HandleMovementStateChanged;
        playerAttack.OnStateChanged += HandleAttackStateChanged;
    }

    void Update()
    {
        // 좌우 이동 입력 받기
        // moveInput = Input.GetAxis("Horizontal");

        if(moveInput != 0)
        {
            // 공격 중이 아닐 때만 방향전환, 이동 가능
            if (!playerAttack.IsAttacking)
            {
                // 방향 설정
                SetDirection();

                // 이동
                movable.Move(moveInput);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        // 공격 입력 처리 (마우스 왼쪽 버튼 클릭)
        if (Input.GetMouseButtonDown(0))
        {
            // playerAttack.TryAttack(moveInput); // 공격 시도
        }
    }

    // 공격 상태 변경에 따른 애니메이션 처리
    private void HandleAttackStateChanged(bool isAttacking)
    {
        if (isAttacking)
        {
            // 공격 애니메이션 재생
            animator.SetTrigger("DoAttack");
        }
        else
        {
            // 공격이 끝난 상태 처리
            animator.ResetTrigger("DoAttack");
        }
    }

    // 이동 상태 변경에 따른 애니메이션 처리
    private void HandleMovementStateChanged(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
    }

    public void SetDirection()
    {
        // 입력과 현재방향이 다르면 Flip
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // 플레이어의 좌우 방향 전환
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1); // 캐릭터 좌우 반전
    }
}