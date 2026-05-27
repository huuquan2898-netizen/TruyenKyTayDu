using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mục tiêu")]
    public Transform player;       // Kéo thả nhân vật Tôn Ngộ Không vào đây
    public float smoothSpeed = 5f; // Độ mượt khi camera bám theo
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Khoảng cách Z luôn phải là số âm để nhìn thấy game 2D

    [Header("Giới hạn Camera")]
    public float minX;         // Giới hạn kịch khung bên trái
    public float maxX = 9999f; // Để số rất lớn vì bên phải muốn đi tiếp sang Scene mới
    public float minY;         // Giới hạn kịch khung bên dưới
    public float maxY;         // Giới hạn kịch khung bên trên

    // Dùng LateUpdate cho Camera để tránh bị giật hình (stuttering) do Player di chuyển ở Update
    void LateUpdate()
    {
        if (player == null) return;

        // 1. Tính toán vị trí camera muốn đi tới
        Vector3 targetPosition = player.position + offset;

        // 2. Chặn tọa độ X và Y không cho vượt quá giới hạn
        // Mathf.Clamp(giá_trị_hiện_tại, nhỏ_nhất, lớn_nhất)
        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

        // 3. Tạo vị trí mới đã được khóa an toàn
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, offset.z);

        // 4. Di chuyển camera mượt mà đến vị trí an toàn
        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
    }
}