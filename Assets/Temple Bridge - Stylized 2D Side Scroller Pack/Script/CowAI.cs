using UnityEngine;
using System.Collections;

public class CowAI : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 2f;

    [Header("Time")]
    public float moveTime = 2f;
    public float idleTime = 2f;

    private Animator anim;

    private bool isMoving = false;
    private bool isFacingRight = true;

    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(AIFlow());
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    // ================= MOVE =================
    void Move()
    {
        float dir = isFacingRight ? 1 : -1;

        transform.Translate(Vector2.right * dir * moveSpeed * Time.deltaTime);

        anim.SetBool("isMoving", true);
    }

    // ================= MAIN LOOP =================
    IEnumerator AIFlow()
    {
        while (true)
        {
            // ===== MOVE 2s =====
            isMoving = true;

            yield return new WaitForSeconds(moveTime);

            // ===== IDLE 2s =====
            isMoving = false;

            anim.SetBool("isMoving", false);

            yield return new WaitForSeconds(idleTime);

            // ===== FLIP =====
            Flip();
        }
    }

    // ================= FLIP =================
    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);

        transform.localScale = scale;
    }
}