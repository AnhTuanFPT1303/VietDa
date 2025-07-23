using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    private bool isOpened = false;
    private Collider2D doorCollider;

    void Start()
    {
        // Tự động lấy Animator component trên cùng đối tượng
        anim = GetComponent<Animator>();
        // Lấy Collider2D của cửa (giả sử là cùng đối tượng)
        doorCollider = GetComponent<Collider2D>();
        // Nếu chưa có chìa khóa, bật collider để chặn player
        if (!GameManager.instance.hasKey)
        {
            doorCollider.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu là Player, có chìa khóa, và cửa chưa mở
        if (other.CompareTag("Player") && GameManager.instance.hasKey && !isOpened)
        {
            // Đánh dấu là đã mở
            isOpened = true;

            // Kích hoạt animation mở cửa
            anim.SetTrigger("Unlock");
            Debug.Log("Cửa đã được mở!");

            // Tắt collider để player đi qua
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }
        }
    }
}