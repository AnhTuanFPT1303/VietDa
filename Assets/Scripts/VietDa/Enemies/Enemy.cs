using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites; // Array of sprites for the enemy
    private Rigidbody2D rb; // Rigidbody for physics interactions
    public float speed; // Speed of the enemy
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)]; // Randomly select a sprite from the array
        float pushX = Random.Range(-1f, 0); // Randomly set the push force in the X direction
        float pushY = Random.Range(-1f, 1f); // Randomly set the push force in the Y direction
        rb.linearVelocity = new Vector2(pushX, pushY); // Set the initial velocity of the enemy to move left
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = speed; // Move left at a constant speed
        transform.position += new Vector3(-moveX * Time.deltaTime, 0, 0); // Move the enemy left over time
        if (transform.position.x < -17) // Check if the enemy has moved off-screen
        {
            Destroy(gameObject); // Destroy the enemy if it goes off-screen
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset the player's position or perform any other logic when colliding with the ground
            Debug.Log("Collided with player");
        }
    }
}
