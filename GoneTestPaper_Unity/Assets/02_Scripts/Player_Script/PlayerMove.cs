using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigid;
    private float horizontalInput;
    private bool isGrounded;

    private bool isSprinting = false;

    [SerializeField] private float fallThreshold = -10f; // 플레이어가 떨어지면 사망 처리할 y 좌표
    [SerializeField] public Transform initialPosition; // 플레이어의 초기 위치를 저장할 변수


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 바닥 체크 (점프 중복 방지)
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);
    }

    void FixedUpdate()
    {
        // 현재 상태에 맞는 속도 선택
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // 좌우 이동 적용
        rigid.linearVelocity = new Vector2(horizontalInput * currentSpeed, rigid.linearVelocity.y);

        // 점프 후 하강 중일 때 중력 가속도를 증가시켜 더 빠르게 떨어지도록 함
        if(rigid.linearVelocity.y < 0)
        {
            // 점프 후 하강 중일 때, 중력 가속도를 증가시켜 더 빠르게 떨어지도록 함
            rigid.gravityScale = 4f;
        }
        else
        {
            // 점프 중이거나 바닥에 있을 때는 기본 중력 적용
            rigid.gravityScale = 2f;
        }

        // 플레이어가 일정 거리 이상 아래로 떨어지면 사망 처리
        dropDie();
    }

    // New Input System: Move 액션이 발생할 때 호출 (SendMessage 또는 Invoke Unity Events 방식)
    public void OnMove(InputValue value)
    {
        // Vector2 값 중 X축(좌우) 입력만 받아옴
        horizontalInput = value.Get<Vector2>().x;
    }

    // New Input System: Jump 액션이 발생할 때 호출
    public void OnJump(InputValue value)
    {
        // 버튼을 '누른 순간'이면서 바닥에 있을 때만 점프
        if (value.isPressed && isGrounded)
        {
            // 순간적인 힘(Impulse)을 위쪽으로 가함
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // New Input System: Sprint 액션이 발생할 때 호출(Shift 키를 누른 상태에서 이동 속도를 증가시키는 기능)
    public void OnSprint(InputValue value)
    {
        // Shift가 눌려있으면 true, 떼지면 false가 저장됨
        isSprinting = value.isPressed;
    }

    // 에디터 뷰에서 바닥 체크 범위를 시각적으로 확인하기 위함
    private void OnDrawGizmos()
    {
        if (groundCheckPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        }
    }

    // 플레이어가 일정거리 이상 아래로 떨어지면 사망 처리
    void dropDie()
    {
        if (transform.position.y < fallThreshold)
        {
            // 사망 처리 로직 (예: 리스폰, 게임 오버 등)
            Debug.Log("Player has fallen and died.");
            // 예시: 플레이어를 초기 위치로 리스폰
            transform.position = initialPosition.position;
            rigid.linearVelocity = Vector2.zero; // 속도 초기화
        }
    }
}