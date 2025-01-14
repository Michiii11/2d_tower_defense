using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    private Rigidbody2D rb;
    private Vector2 velocity;

    public float raycastDistance = 1.0f; // Distance for checking obstacles above

    // Start is called before the first execution of Update
    void Start()
    {
        // Add or get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
        rb.gravityScale = 0;

        // Add a Collider2D if missing
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        // Set initial values
        transform.position = new Vector3(0, 0, -1);
        velocity = Vector2.up * speed;
    }

    void Update()
    {
        // Perform a raycast to check for obstacles directly above
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance);

        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name + ", Tag: " + hit.collider.gameObject.tag);

            if (hit.collider.CompareTag("Block"))
            {
                Debug.Log("Raycast confirmed Block tag.");
                // Handle blocked path logic here
            }
        }
        else
        {
            // Path above is clear, move upward
            velocity = Vector2.up * speed;
        }

        // Apply the velocity to the Rigidbody2D
        rb.linearVelocity = velocity;

        Debug.Log("Enemy Velocity: " + velocity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Collision with Block detected!");

            // Randomly choose to move left or right if blocked above
            if (velocity == Vector2.up)
            {
                velocity = Vector2.right;
            }
            else if (velocity == Vector2.right)
            {
                velocity = Vector2.up;
            }
        }
    }
}
