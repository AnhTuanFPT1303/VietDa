using UnityEngine;
using System.Collections;

public class Slime_bomb : MonoBehaviour
{
    public float explosionRadius = 1f;      // Explosion radius in tiles
    public int pushDistance = 7;            // How many blocks to push the player
    public float pushForce = 20f;           // Force to push players
    public GameObject explosionEffectPrefab; // Visual effect for explosion
    private Knockback knockback;

    void Start()
    {
        // Initialize the Knockback component
        knockback = GetComponent<Knockback>();
        if (knockback == null)
        {
            Debug.LogWarning("Knockback component not found on the bomb!");
        }
    }

    void Update()
    {
    }

    public void Explode()
    {
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

        // Destroy the bomb
        Destroy(gameObject);
    }

    private void CheckAndPush(Vector2 direction)
    {
        Vector2 checkPos = (Vector2)transform.position + (direction * explosionRadius);

        // Find objects at this position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPos, 0.5f);

        foreach (Collider2D hit in colliders)
        {
            // Check if it's a player
            if (hit.CompareTag("Player"))
            {
                Rigidbody2D playerRb = hit.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                        // Check if player has a Knockback component
                        Knockback playerKnockback = hit.GetComponent<Knockback>();
                        if (playerKnockback != null)
                        {
                            playerKnockback.PlayKnockback(gameObject);
                        }
                }
            }
        }
    }
}