using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    private Direction movementDirection = Direction.UP;
    private Rigidbody2D rb;
    public GridManager gridManager;

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

    void Update(){

        if(GameState.Instance.playMode != PlayMode.PLAY) return;
        

        //walk forward
        if (movementDirection == Direction.UP)
        {
            rb.linearVelocityY = speed;
            //Debug.Log("Enemy Velocity: " + rb.linearVelocityY); // Log current velocity
        }

        //walk right
        if (movementDirection == Direction.RIGHT)
        {
            rb.linearVelocityX = speed;
            //Debug.Log("Enemy Velocity: " + rb.linearVelocityY); // Log current velocity
        }
        
        //walk left
        if (movementDirection == Direction.LEFT)
        {
            rb.linearVelocityX = -speed;
            //Debug.Log("Enemy Velocity: " + rb.linearVelocityY); // Log current velocity
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Vector2 topLeft = spriteRenderer.bounds.min;

            Vector2 topRight = new Vector2(spriteRenderer.bounds.max.x, spriteRenderer.bounds.min.y);

            Vector2 gridPosition = gridManager.GetCoordinatesOfTile(gridManager.GetTileAtWorldPosition
            (topLeft)).Value;

            Vector2 tileAbovePosition = new Vector2(gridPosition.x, gridPosition.y + 1); 

            Tile tileAbove = gridManager.GetTileByCoordinates((int)tileAbovePosition.x, (int)tileAbovePosition.y);

            if (tileAbove != null)
            {
                Debug.Log("Tile above found: " + tileAbove.name);
            }
            else
            {
                Debug.LogWarning("No tile found above the current tile.");
            }
        }
        else
        {
            // Path above is clear, move upward
            velocity = Vector2.up * speed;
        }

        //Debug.Log("Enemy Position: " + transform.position); // Log position to track movement
    }

    void OnCollisionEnter2D(Collision2D collision){
        Direction collisionDirection = CalculateColisionDirection(collision);

        Debug.Log(collisionDirection);

        if(movementDirection == Direction.UP && collisionDirection == Direction.UP){
            rb.linearVelocityY = 0;
            Vector2 newPosition = rb.position + new Vector2(0, -0.1f);
            rb.MovePosition(newPosition);

            movementDirection = Direction.RIGHT;
        }

        if(movementDirection == Direction.RIGHT && collisionDirection == Direction.RIGHT){
            rb.linearVelocityX = 0;
            //Vector2 newPosition = rb.position + new Vector2(-0.1f, 0);

            movementDirection = Direction.LEFT;
        }

        if(movementDirection == Direction.LEFT && collisionDirection == Direction.LEFT){
            rb.linearVelocityX = 0;
            //Vector2 newPosition = rb.position + new Vector2(-0.1f, 0);

            movementDirection = Direction.RIGHT;
        }

        Debug.Log("Collision detected! " + collision.gameObject.name + " ... ");
        
        rb.linearVelocityY = 0;
    }

    Direction CalculateColisionDirection(Collision2D collision){
        ContactPoint2D contact = collision.contacts[0];
        
        // Calculate the collision direction
        Vector2 collisionDirection = contact.point - (Vector2)transform.position;
        collisionDirection.Normalize();

        Debug.Log("Collision Direction: " + collisionDirection);

        if (movementDirection == Direction.RIGHT || movementDirection == Direction.LEFT)
        {
            if (collisionDirection.x > 0)
                return Direction.RIGHT;
            else
                return Direction.LEFT;
        }
        else
        {
            if (collisionDirection.y > 0)
                return Direction.UP;
            else
                return Direction.DOWN;
        }
    }
}

public enum Direction{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
