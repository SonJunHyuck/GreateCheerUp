using System;
using Unity.Burst;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    private IMovable movable; // IInputMovable 인터페이스 참조

    private float moveInput;            // 입력 값을 저장

    public bool isFacingRight = true; // 기본 방향 (좌측)

    public Action<float, float> onSetEnergy;
    [SerializeField] float playerMaxEnergy;
    [SerializeField] float playerEnergyChargySpeed;
    float playerCurrentEnergy;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // IInputMovable 인터페이스를 구현한 컴포넌트를 가져옴
        movable = GetComponent<IMovable>();

        movable.OnStateChanged += HandleMovementStateChanged;

        playerCurrentEnergy = 0;
    }

    private void OnEnable()
    {
        Observer.Instance.onRequestMovePlayer += ResponseSetMoveInput;
        Observer.Instance.onRequestConsumeEnergy += ConsumeEnergy;
    }

    private void OnDisable()
    {
        Observer.Instance.onRequestMovePlayer -= ResponseSetMoveInput;
        Observer.Instance.onRequestConsumeEnergy -= ConsumeEnergy;
    }

    void Update()
    {
        if(moveInput != 0)
        {
            // 방향 설정
            SetDirection();

            // 이동
            movable.Move(moveInput);
        }
        else
        {
            HandleMovementStateChanged(false);
        }
    }

    private void FixedUpdate() 
    {
        playerCurrentEnergy += Time.fixedDeltaTime * playerEnergyChargySpeed;
        playerCurrentEnergy = Mathf.Clamp(playerCurrentEnergy, 0, playerMaxEnergy);
        onSetEnergy?.Invoke(playerMaxEnergy, playerCurrentEnergy);
    }

    // 이동 상태 변경에 따른 애니메이션 처리
    private void HandleMovementStateChanged(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
    }

    // UI에서 호출되는 메서드: 이동 입력값 설정 (버튼으로 입력값 전달)
    public void ResponseSetMoveInput(float input)
    {
        moveInput = input;
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

    private bool ConsumeEnergy(int cost)
    {
        if(playerCurrentEnergy >= cost)
        {
            playerCurrentEnergy -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetEnergy()
    {
        onSetEnergy?.Invoke(playerMaxEnergy, playerCurrentEnergy);
    }
}