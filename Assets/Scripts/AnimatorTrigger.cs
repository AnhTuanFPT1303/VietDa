using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    // Tên của Trigger trong Animator Controller
    public string triggerName = "StartWaving";

    // Biến để đảm bảo animation chỉ chạy một lần
    private bool hasTriggered = false;

    // Lấy component Animator để điều khiển
    private Animator animator;

    void Start()
    {
        // Tự động lấy Animator component trên cùng một đối tượng
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player và animation chưa được kích hoạt
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // Đánh dấu là đã kích hoạt để không chạy lại
            hasTriggered = true;

            // Kích hoạt Trigger trong Animator
            animator.SetTrigger(triggerName);
        }
    }
}
