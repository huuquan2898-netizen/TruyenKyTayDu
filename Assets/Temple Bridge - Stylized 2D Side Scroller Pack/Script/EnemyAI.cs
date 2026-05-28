using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 2f;

    [Header("Time")]
    public float moveTime = 2f;
    public float idleTime = 2f;

    [Header("Attack")]
    public Transform firePoint;
    public GameObject firePrefab;

    private Animator anim;

    private bool isFacingRight = true;
    private bool isAttacking = false;
    private bool isMoving = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(AIFlow());
    }

    void Update()
    {
        if (isMoving && !isAttacking)
        {
            Move();
        }
        else
        {
            // luôn đảm bảo đứng yên khi không move
            anim.SetBool("isMoving", false);
        }
    }

    // ================= MOVE =================
    void Move()
    {
        float dir = isFacingRight ? 1 : -1;

        transform.Translate(Vector2.right * dir * moveSpeed * Time.deltaTime);

        anim.SetBool("isMoving", true);
        anim.SetBool("isIdle", false);
    }

    // ================= MAIN FLOW =================
    IEnumerator AIFlow()
    {
        while (true)
        {
            // ================= 1. WALK 2s =================
            isMoving = true;
            anim.SetBool("isIdle", false);

            yield return new WaitForSeconds(moveTime);

            // STOP → IDLE
            isMoving = false;

            anim.SetBool("isMoving", false);
            anim.SetBool("isIdle", true);

            // ================= 2. IDLE 2s =================
            yield return new WaitForSeconds(idleTime);

            anim.SetBool("isIdle", false);

            // ================= 3. ATTACK =================
            yield return StartCoroutine(Attack());

            // ================= 4. IDLE SAU ATTACK =================
            anim.SetBool("isIdle", true);
            yield return new WaitForSeconds(idleTime);
            anim.SetBool("isIdle", false);

            // ================= 5. FLIP =================
            Flip();
        }
    }

    // ================= ATTACK =================
    IEnumerator Attack()
    {
        isAttacking = true;
        isMoving = false;

        // đảm bảo đứng yên hoàn toàn
        anim.SetBool("isMoving", false);

        anim.SetTrigger("isAttack");

        // chờ animation phun lửa (FireEvent sẽ bắn)
        yield return new WaitForSeconds(1f);

        isAttacking = false;
    }

    // ================= ANIMATION EVENT =================
    public void FireEvent()
    {
        if (firePrefab == null || firePoint == null) return;

        GameObject fire = Instantiate(firePrefab, firePoint.position, Quaternion.identity);

        FireProjectile fp = fire.GetComponent<FireProjectile>();
        fp.SetDirection(isFacingRight);
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