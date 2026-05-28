using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 7f;
    public float lifeTime = 3f;

    private float directionX;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(bool facingRight)
    {
        directionX = facingRight ? 1f : -1f;

        // 🔥 LẬT SPRITE LỬA CHO ĐÚNG HƯỚNG
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * directionX;
        transform.localScale = scale;
    }

    void Update()
    {
        transform.Translate(Vector2.right * directionX * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}