using UnityEngine;
using System.Collections;

public class EnemyGoal : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    void Start()
    {
        if (_cam == null)
            _cam = Camera.main; // Assign main camera if not set

        StartCoroutine(WaitAndExecute());
    }

    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(0.1f); // Wait for 100ms
        
        CreateEdgeColliders();
    }

   void CreateEdgeColliders()
    {
        // Calculate the screen size in world units
        float screenHeight = _cam.orthographicSize * 2f;  // Height of the screen in world units
        float screenWidth = screenHeight * _cam.aspect;   // Width of the screen in world units

        // Get the camera's position in world space
        Vector3 camPosition = _cam.transform.position;

        // Bottom Edge
        CreateCollider(new Vector2(camPosition.x, camPosition.y + screenHeight / 2f + 0.5f), new Vector2(screenWidth, 1f));
    }

    void CreateCollider(Vector2 position, Vector2 size)
    {
        GameObject edge = gameObject;
        edge.transform.position = position;

        BoxCollider2D collider = edge.AddComponent<BoxCollider2D>();
        collider.size = size;

        collider.isTrigger = true;

        // Optional: Add a Rigidbody2D for better physics interactions
        Rigidbody2D rb = edge.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static; // Set Rigidbody to Static

        edge.transform.parent = transform;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameState.Instance.enemyPassedThrough();

        // Destroy the object that entered the trigger
        Destroy(collider.gameObject);
    }
}
