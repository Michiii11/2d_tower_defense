using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    private bool isStopped = false;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add or get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Dynamic; // Use Dynamic for physics-based movement
        rb.freezeRotation = true;
        rb.gravityScale = 0; // Disable gravity

        // Add a Collider2D if missing
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>(); // Adjust to fit your object
        }

        rb.linearVelocityY = 0;
        transform.position = new Vector3(0, 0, 0); 
    }

    void Update()
    {
        if(GameState.Instance.playMode == PlayMode.PLAY) {
            if (!isStopped)
            {
                // Move the enemy upwards using linearVelocity
                rb.linearVelocityY = speed;
                Debug.Log("Enemy Velocity: " + rb.linearVelocityY); // Log current velocity
            }
            else
            {
                // Stop movement
                rb.linearVelocityY = 0;
                Debug.Log("Enemy stopped. Velocity: " + rb.linearVelocityY); // Log when stopped
            }
        }

        Debug.Log("Enemy Position: " + transform.position); // Log position to track movement
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected! " + collision.gameObject.name + " ... " + isStopped);
        {
            Debug.Log("Collision with Block detected!");
            isStopped = true;
            rb.linearVelocityY = 0;
        }
    }
}