using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public CharacterController characterController; 
    private PlayerInput playerInput;
    public float moveSpeed = 20f;
    public Animator animator;
    BoxCollider2D playerCollider; 
    private Rigidbody2D rb;
    [SerializeField] GameObject bombObject;
    public InputAction MoveAction { get; set; }
    public InputAction BombAction { get; set; }
    private List<GameObject> placedBombs = new List<GameObject>();
    public bool canMove = true;
    private float normalGravity = 3;
    private float airGravity = 35;
    public bool isBeingKnockbacked = false; // Flag controlled by Knockback script


    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>(); 
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        MoveAction = playerInput.actions["Move"];
        BombAction = playerInput.actions["Bomb"];
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
        OnLanding();
        Bomb();
        UpdateGravity();
    }

    private void Move()
    {
        if (!canMove) return;

        if (MoveAction != null)
        {
            Vector2 input = MoveAction.ReadValue<Vector2>();
            rb.linearVelocity = input * moveSpeed;

            if (input.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = input.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }

    private void UpdateGravity()
    {
        if (isBeingKnockbacked)
        {
            rb.gravityScale = normalGravity; // Bomb knockback always uses normal gravity
        }
        else if (!IsGrounded())
        {
            rb.gravityScale = airGravity; // In air without knockback
        }
        else
        {
            rb.gravityScale = normalGravity; // Default when grounded
        }
    }

    private bool IsGrounded()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || playerCollider.IsTouchingLayers(LayerMask.GetMask("Bomb")))
            return true;
        else return false;
    }

    private void Bomb()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            // Get the player's current position
            Vector2 playerPosition = transform.position;
            
            // Spawn bomb at player's position
            GameObject newBomb = Instantiate(bombObject, playerPosition, Quaternion.identity);
            
            // Add bomb to our list to track it
            placedBombs.Add(newBomb);
            
            // Calculate position above the bomb based on collider sizes
            float playerHeight = playerCollider.bounds.size.y;
            float bombHeight = 0;
            
            // Try to get the bomb's collider if it has one
            Collider2D bombCollider = newBomb.GetComponent<Collider2D>();
            if (bombCollider != null)
            {
                bombHeight = bombCollider.bounds.size.y;
            }
            
            // Move player to position above bomb (with slight offset to prevent collision)
            Vector2 newPosition = playerPosition + new Vector2(0, (bombHeight + playerHeight) / 2);
            transform.position = newPosition;
            
            // Add a small upward impulse to make it feel more natural
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            // Detonate all bombs this player has placed
            foreach (GameObject bomb in placedBombs.ToArray())
            {
                if (bomb != null)
                {
                    // Get the Slime_bomb component and call Explode()
                    Slime_bomb bombScript = bomb.GetComponent<Slime_bomb>();
                    if (bombScript != null)
                    {
                        bombScript.Explode();
                    }
                }
            }

            // Clear the list since all bombs have been detonated
            placedBombs.Clear();
        }
    }

    void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
}