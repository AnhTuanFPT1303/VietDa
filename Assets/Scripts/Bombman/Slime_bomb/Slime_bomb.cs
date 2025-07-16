using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps; // Bắt buộc phải có để làm việc với Tilemap

public class Slime_bomb : MonoBehaviour
{
    public int explosionTileRadius = 1;      // Bán kính nổ (tính bằng số ô gạch)
    public GameObject explosionEffectPrefab; // Prefab hiệu ứng nổ (tùy chọn)
    [Header("Cài đặt Đẩy Lùi")]
    public float pushForce = 20f;           // Lực đẩy người chơi
    public float explosionRadius = 1f;      // Explosion radius in tiles
    public int pushDistance = 7;            // How many blocks to push the player
    public float pushForce = 20f;           // Force to push players
    public GameObject explosionEffectPrefab; // Visual effect for explosion
    private Knockback knockback;
    private Animator animator;
    AudioSource audioSource;
    void Start()
    {
        // Initialize the Knockback component
        knockback = GetComponent<Knockback>();
        if (knockback == null)
        {
            Debug.LogWarning("Knockback component not found on the bomb!");
        }
        audioSource = GetComponent<AudioSource>();
        // Initialize the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on the bomb!");
        }
    }

    // Hàm này được gọi khi script được khởi tạo
    void Start()
    {
        // Bạn có thể thêm các khởi tạo khác ở đây nếu cần
    }

    /// <summary>
    /// Kích hoạt vụ nổ. Hàm này sẽ được gọi từ một script khác (ví dụ: khi người chơi đặt bom).
    /// </summary>
    public void Explode()
    {
        // Phá hủy các ô gạch trong bán kính
        DestroyTilesInRadius();

        // Đẩy lùi người chơi trong bán kính
        PushPlayersInRadius();

        // Hiển thị hiệu ứng nổ nếu có
        // Update scale to (7, 7, current z) when exploding
        transform.localScale = new Vector3(7f, 7f, transform.localScale.z);
        audioSource.Play(); // Play explosion sound
        // Trigger the explosion animation
        if (animator != null)
        {
            animator.SetBool("IsExploding", true);
        }

        // Create plus-shaped explosion pattern
        CheckAndPush(Vector2.up);
        CheckAndPush(Vector2.down);
        CheckAndPush(Vector2.left);
        CheckAndPush(Vector2.right);
        CheckAndPush(Vector2.zero); // Center point

        // Show explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the bomb after a short delay to allow animation to play
        Destroy(gameObject, 1f);
    }

    /// <summary>
    /// Tìm và phá hủy tất cả các ô gạch "CanDestroy" trong bán kính nổ.
    /// </summary>
    private void DestroyTilesInRadius()
    {
        // Tìm đối tượng có tag "CanDestroy" để lấy Tilemap
        GameObject destructibleObject = GameObject.FindWithTag("CanDestroy");
        if (destructibleObject == null) return; // Không tìm thấy thì bỏ qua

        Tilemap destructibleTilemap = destructibleObject.GetComponent<Tilemap>();
        if (destructibleTilemap == null) return; // Không có component Tilemap thì bỏ qua

        // Lấy vị trí của quả bom trên lưới tilemap
        Vector3Int originCell = destructibleTilemap.WorldToCell(transform.position);

        // Duyệt qua một vùng hình vuông xung quanh quả bom
        for (int x = -explosionTileRadius; x <= explosionTileRadius; x++)
        {
            for (int y = -explosionTileRadius; y <= explosionTileRadius; y++)
            {
                // Nếu bạn muốn vụ nổ hình tròn thay vì vuông, bỏ comment dòng dưới
                // if (new Vector2(x, y).magnitude > explosionTileRadius) continue;

                // Lấy vị trí ô gạch mục tiêu
                Vector3Int targetCell = originCell + new Vector3Int(x, y, 0);

                // Nếu có gạch ở ô đó, phá hủy nó
                if (destructibleTilemap.GetTile(targetCell) != null)
                {
                    destructibleTilemap.SetTile(targetCell, null);
                }
            }
        }
    }

    /// <summary>
    /// Tìm và đẩy lùi tất cả người chơi trong bán kính nổ.
    /// </summary>
    private void PushPlayersInRadius()
    {
        // Chuyển đổi bán kính từ ô gạch sang đơn vị của thế giới game
        float worldRadius = explosionTileRadius + 0.5f;

        // Tìm tất cả các collider trong phạm vi nổ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, worldRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                Knockback playerKnockback = hit.GetComponent<Knockback>();
                if (playerKnockback != null)
                {
                    playerKnockback.PlayKnockback(gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Hàm này chỉ chạy trong Editor, dùng để vẽ một vòng tròn đỏ giúp bạn
    /// thấy được phạm vi nổ một cách trực quan.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float worldRadius = explosionTileRadius + 0.5f;
        Gizmos.DrawWireSphere(transform.position, worldRadius);
    }
}
