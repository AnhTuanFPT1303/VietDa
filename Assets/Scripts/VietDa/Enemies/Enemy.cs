using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    CircleCollider2D enemyCollider; // Collider for the enemy
    [SerializeField] private Sprite[] sprites; // Array of sprites for the enemy
    private Rigidbody2D rb; // Rigidbody for physics interactions
    public float speed = 5f; // Speed of the enemy
    void Start()
    {
        enemyCollider = GetComponent<CircleCollider2D>(); // Get the CircleCollider2D component attached to the enemy
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        //spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)]; // Randomly select a sprite from the array
        float pushX = Random.Range(-1f, 0); // Randomly set the push force in the X direction
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = speed; // Move left at a constant speed
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < -17) // Check if the enemy has moved off-screen
        {
            Destroy(gameObject); // Destroy the enemy if it goes off-screen
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Enter with: " + collision.gameObject.name);

        if (collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            Debug.Log("Enemy collided with Player");
            Destroy(gameObject);
        }
    }
}
