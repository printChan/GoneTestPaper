using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public float moveSpeed = 5f;

    Vector2 moveInput;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // immediateMove();
        softMove();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

// -----------------------------------------------------------------------------------------------

    // 즉각적인 움직임 구현
    void immediateMove()
    {
        rigid.linearVelocity = new Vector2(moveInput.x * moveSpeed, rigid.linearVelocity.y);
    }

    // 부드러운 움직임 구현
    void softMove()
    {
        rigid.AddForce(new Vector2(moveInput.x * moveSpeed, 0f), ForceMode2D.Impulse);

        if (rigid.linearVelocity.x > moveSpeed)
        {
            rigid.linearVelocity = new Vector2(moveSpeed, rigid.linearVelocity.y);
        }
        else if (rigid.linearVelocity.x < -moveSpeed)
        {
            rigid.linearVelocity = new Vector2(-moveSpeed, rigid.linearVelocity.y);
        }
    }
}
