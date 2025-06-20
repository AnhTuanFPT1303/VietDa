using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private PlayerInput playerInput;
    private InputAction moveAction;
    public float flySpeed = 5f;
    public InputAction HoldAction { get; set; }
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if HoldAction is not null and is being performed
        if (HoldAction != null && HoldAction.IsPressed())
        {
            // Move the player upward gradually
            transform.position += Vector3.up * flySpeed * Time.deltaTime;

            // Neutralize gravity effect
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
                rb.gravityScale = 0; // Disable gravity while flying (2D)
                // rb.useGravity = false; // Uncomment for 3D physics
            }
        }
        else if (rb != null)
        {
            // Re-enable gravity when not flying
            rb.gravityScale = 1; // Normal gravity (2D)
            // rb.useGravity = true; // Uncomment for 3D physics
        }
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        HoldAction = playerInput.actions["Fly"];
    }
}
