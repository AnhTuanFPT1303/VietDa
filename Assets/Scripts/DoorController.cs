using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    private bool isOpened = false; // Cờ để đảm bảo cửa chỉ mở một lần

    void Start()
    {
        // Tự động lấy Animator component trên cùng đối tượng
        anim = GetComponent<Animator>();
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
        }
    }
}