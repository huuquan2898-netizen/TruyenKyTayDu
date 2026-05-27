using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction JumpAction;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        MoveAction.Enable();
        JumpAction.Enable();
    }

    void Update()
    {
        Move();
        Jump();

        // Animation
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // Nếu đang rơi hoặc bay lên
        if (Mathf.Abs(rb.linearVelocity.y) > 0.1f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    void Move()
    {
        Vector2 move = MoveAction.ReadValue<Vector2>();

        rb.linearVelocity = new Vector2(
            move.x * moveSpeed,
            rb.linearVelocity.y
        );

        // Flip nhân vật
        if (move.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (move.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Jump()
    {
        if (JumpAction.triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }
    }
}