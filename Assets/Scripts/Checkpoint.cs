using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private bool hasBeenActivated = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ kích hoạt nếu là Player và chưa được kích hoạt trước đó
        if (other.CompareTag("Player") && !hasBeenActivated)
        {
            // Thông báo cho GameManager để cập nhật điểm hồi sinh
            GameManager.instance.UpdateCheckpoint(this);

            // Kích hoạt animation của cờ (nếu có)
            if (anim != null)
            {
                anim.SetTrigger("StartWaving"); // Giả sử bạn có trigger tên là "StartWaving"
            }
        }
    }

    // Hàm này được gọi bởi GameManager để đánh dấu cờ này đã được kích hoạt
    public void Activate()
    {
        hasBeenActivated = true;
    }

    // Hàm này được gọi bởi GameManager để vô hiệu hóa các cờ khác
    public void Deactivate()
    {
        hasBeenActivated = false;
        // Bạn có thể thêm logic để reset animation của cờ về trạng thái đứng yên ở đây
    }
}