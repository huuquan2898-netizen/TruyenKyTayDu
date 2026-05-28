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
            // đứng yên
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

    // ================= MAIN FLOW =================
    IEnumerator AIFlow()
    {
        while (true)
        {
            // ===== WALK =====
            isMoving = true;

            yield return new WaitForSeconds(moveTime);

            // ===== IDLE =====
            isMoving = false;

            anim.SetBool("isMoving", false);

            yield return new WaitForSeconds(idleTime);

            // ===== ATTACK =====
            yield return StartCoroutine(Attack());

            // ===== IDLE SAU ATTACK =====
            anim.SetBool("isMoving", false);

            yield return new WaitForSeconds(idleTime);

            // ===== FLIP =====
            Flip();
        }
    }

    // ================= ATTACK =================
    IEnumerator Attack()
    {
        isAttacking = true;
        isMoving = false;

        anim.SetBool("isMoving", false);

        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(1f);

        isAttacking = false;
    }

    // ================= FIRE EVENT =================
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