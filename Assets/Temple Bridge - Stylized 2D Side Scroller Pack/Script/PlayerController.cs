using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction JumpAction;
    public InputAction AttackAction;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = true;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        MoveAction.Enable();
        JumpAction.Enable();
        AttackAction.Enable();
    }

    void Update()
    {
        HandleAttack();

        if (!isAttacking)
        {
            Move();
            Jump();
        }

        UpdateAnimation();
    }

    // ================= MOVE =================
    void Move()
    {
        Vector2 move = MoveAction.ReadValue<Vector2>();

        rb.linearVelocity = new Vector2(
            move.x * moveSpeed,
            rb.linearVelocity.y
        );

        // Flip
        if (move.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // ================= JUMP =================
    void Jump()
    {
        if (JumpAction.triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    // ================= ATTACK =================
    void HandleAttack()
    {
        if (AttackAction.triggered && !isAttacking)
        {
            isAttacking = true;

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            animator.SetTrigger("Attack");
        }
    }

    // ================= ANIMATION RESET =================
    public void EndAttack()
    {
        isAttacking = false;
    }

    // ================= ANIMATION =================
    void UpdateAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGrounded);
    }

    // ================= GROUND CHECK =================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}