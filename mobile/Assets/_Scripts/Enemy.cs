using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    private bool isStopped = false;
    private Rigidbody2D rb;

    void Start()
    {
        // Add or get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0; // Disable gravity
        rb.bodyType = RigidbodyType2D.Dynamic; // Use Dynamic for physics-based movement

        // Add a Collider2D if missing
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>(); // Adjust to fit your object
        }

        transform.position = new Vector3(0, 0, -1);
    }

    void Update()
    {
        if (!isStopped)
        {
            // Move the enemy upwards using velocity
            rb.linearVelocity = new Vector2(0, speed);
        }
        else
        {
            // Stop movement
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected! " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Collision with Block detected!");
            isStopped = true;
        }
    }
}
