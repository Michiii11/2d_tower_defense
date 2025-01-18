using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed;
    private int health = 100;
    private Direction movementDirection = Direction.UP;
    private Rigidbody2D rb;
    public GridManager gridManager;

    public float raycastDistance = 1.0f; // Distance for checking obstacles above

    public void Init(GridManager gridManager){
        this.gridManager = gridManager;
    }

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
    }

    void Update(){

        if(GameState.Instance.playMode != PlayMode.PLAY) return;
        

        //walk forward
        if (movementDirection == Direction.UP){
            rb.linearVelocityY = speed;
            rb.linearVelocityX = 0;
        }
        else{
            rb.linearVelocityY = 0;
        }

        //walk right
        if (movementDirection == Direction.RIGHT){
            rb.linearVelocityX = speed;
        }
        
        //walk left
        if (movementDirection == Direction.LEFT){
            rb.linearVelocityX = -speed;
        }

        Vector2 currentPos;

        if(movementDirection == Direction.LEFT){
            currentPos = new Vector2(gameObject.transform.position.x+0.9f, gameObject.transform.position.y);
        }
        else if(movementDirection == Direction.RIGHT){
            currentPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        }
        else{
            currentPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        }

        Debug.Log("Current Position: " + currentPos);

        GameState.DrawDebugDot(currentPos);

        Vector2 tileAbovePosition = currentPos - new Vector2(0, -1);

        Tile tileAbove = gridManager.GetTileByCoordinates((int)tileAbovePosition.x, (int)tileAbovePosition.y);

        if (tileAbove != null)
        {
            Debug.Log("Tile above found: " + tileAbove.name);

            if(tileAbove.tileType == TileType.EMPTY){
                movementDirection = Direction.UP;
                Debug.Log("empty: Moving Up");
            }
        }
        else
        {
            Debug.LogWarning("No tile found above the current tile.");
        }
        
        //Debug.Log("Enemy Position: " + transform.position); // Log position to track movement
    }

    public void DealDamage(int damage){
        health -= damage;
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        Direction collisionDirection = CalculateColisionDirection(collision);

        Debug.Log(collisionDirection);
        
        System.Random random = new System.Random();

        if(movementDirection == Direction.UP && collisionDirection == Direction.UP){
            rb.linearVelocityY = 0;
            Vector2 newPosition = rb.position + new Vector2(0, -0.1f);
            rb.MovePosition(newPosition);

            int randNum = random.Next(1, 10);

            if(randNum <= 3){
                movementDirection = Direction.RIGHT;
            }
            else if(randNum <= 6){
                movementDirection = Direction.LEFT;
            }
            else{
                movementDirection = Direction.STILL;
                StartCoroutine(BreakWall(collision.gameObject.GetComponent<Tile>()));
            }
        }

        if(movementDirection == Direction.RIGHT && collisionDirection == Direction.RIGHT){
            rb.linearVelocityX = 0;
            Vector2 newPosition = rb.position + new Vector2(-0.1f, 0);
            rb.MovePosition(newPosition);
            
            int randNum = random.Next(1, 10);

            if(randNum <= 3){
                movementDirection = Direction.STILL;
                StartCoroutine(BreakWall(collision.gameObject.GetComponent<Tile>()));
            }
            else{
                movementDirection = Direction.LEFT;
            }
        }

        if(movementDirection == Direction.LEFT && collisionDirection == Direction.LEFT){
            rb.linearVelocityX = 0;
            Vector2 newPosition = rb.position + new Vector2(0.1f, 0);
            rb.MovePosition(newPosition);

            int randNum = random.Next(1, 10);

            if(randNum <= 3){
                movementDirection = Direction.STILL;
                StartCoroutine(BreakWall(collision.gameObject.GetComponent<Tile>()));
            }
            else{
                movementDirection = Direction.RIGHT;
            }
        }

        Debug.Log("Collision detected! " + collision.gameObject.name);
        
        rb.linearVelocityY = 0;
    }

    IEnumerator BreakWall(Tile tile)
    {
        yield return new WaitForSeconds(3f);
        tile.RemoveWall();
        movementDirection = Direction.UP;
    }

    private Direction CalculateColisionDirection(Collision2D collision){
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
    RIGHT,
    STILL
}
