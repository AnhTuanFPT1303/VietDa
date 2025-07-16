using UnityEngine;
using System.Collections;

public class BidirectionalTeleporter : MonoBehaviour
{
    // Enum để chọn trục sẽ dùng để xác định hướng đi
    public enum DetectionAxis { Horizontal, Vertical }

    [Header("Cài đặt chung")]
    public DetectionAxis axis = DetectionAxis.Horizontal; // Chọn trục dịch chuyển

    [Header("Cài đặt cho Chiều A -> B (Trái->Phải hoặc Dưới->Lên)")]
    public Transform playerDestination_B;
    public Camera cameraToEnable_B;
    public Camera cameraToDisable_A;

    [Header("Cài đặt cho Chiều B -> A (Phải->Trái hoặc Trên->Dưới)")]
    public Transform playerDestination_A;
    public Camera cameraToEnable_A;
    public Camera cameraToDisable_B;

    // Biến cờ để ngăn chặn việc dịch chuyển liên tục
    private bool canTeleport = true;

    /// <summary>
    /// Được gọi khi một đối tượng đi vào vùng trigger.
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ thực hiện nếu đối tượng là Player và có thể dịch chuyển
        if (other.CompareTag("Player") && canTeleport)
        {
            StartCoroutine(TeleportSequence(other.transform));
        }
    }

    /// <summary>
    /// Thực hiện tuần tự các hành động dịch chuyển.
    /// </summary>
    private IEnumerator TeleportSequence(Transform player)
    {
        // Vô hiệu hóa việc dịch chuyển tạm thời
        canTeleport = false;

        // --- LOGIC MỚI: XÁC ĐỊNH HƯỚNG DỰA TRÊN VỊ TRÍ TƯƠNG ĐỐI ---
        if (axis == DetectionAxis.Horizontal)
        {
            // So sánh theo trục X (Ngang)
            if (player.position.x < transform.position.x)
            {
                // Người chơi ở bên trái -> Dịch chuyển sang B (bên phải)
                TeleportTo(player, playerDestination_B, cameraToEnable_B, cameraToDisable_A);
            }
            else
            {
                // Người chơi ở bên phải -> Dịch chuyển sang A (bên trái)
                TeleportTo(player, playerDestination_A, cameraToEnable_A, cameraToDisable_B);
            }
        }
        else // axis == DetectionAxis.Vertical
        {
            // So sánh theo trục Y (Dọc)
            if (player.position.y < transform.position.y)
            {
                // Người chơi ở bên dưới -> Dịch chuyển lên B (bên trên)
                TeleportTo(player, playerDestination_B, cameraToEnable_B, cameraToDisable_A);
            }
            else
            {
                // Người chơi ở bên trên -> Dịch chuyển xuống A (bên dưới)
                TeleportTo(player, playerDestination_A, cameraToEnable_A, cameraToDisable_B);
            }
        }

        // Đợi một chút để người chơi di chuyển ra khỏi trigger
        yield return new WaitForSeconds(0.2f);

        // Cho phép dịch chuyển trở lại
        canTeleport = true;
    }

    /// <summary>
    /// Hàm hỗ trợ để thực hiện việc dịch chuyển và đổi camera.
    /// </summary>
    private void TeleportTo(Transform player, Transform destination, Camera camToEnable, Camera camToDisable)
    {
        player.position = destination.position;
        if (camToEnable != null) camToEnable.gameObject.SetActive(true);
        if (camToDisable != null) camToDisable.gameObject.SetActive(false);
    }
}
