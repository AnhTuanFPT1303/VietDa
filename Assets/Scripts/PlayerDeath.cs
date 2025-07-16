using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    // Hàm này được gọi khi có va chạm vật lý xảy ra
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player va chạm với: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
        // Kiểm tra xem có va chạm với đối tượng có tag "Lava" không
        if (collision.gameObject.CompareTag("Lava"))
        {
            // Gọi hàm hồi sinh từ GameManager
            GameManager.instance.RespawnPlayer();
        }
    }
}