using UnityEngine;
using System.Collections;

public class Slime_bomb : MonoBehaviour
{
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

    void Update()
    {
    }

    public void Explode()
    {
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