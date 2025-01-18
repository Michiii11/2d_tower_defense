using UnityEngine;
using System.Collections;

public class ScreenEdgeColliders : MonoBehaviour
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

        // Left Edge
        CreateCollider(new Vector2(camPosition.x - screenWidth / 2f - 0.5f, camPosition.y), new Vector2(1f, screenHeight));

        // Right Edge
        CreateCollider(new Vector2(camPosition.x + screenWidth / 2f + 0.5f, camPosition.y), new Vector2(1f, screenHeight));

        // Bottom Edge
        CreateCollider(new Vector2(camPosition.x, camPosition.y - screenHeight / 2f - 0.5f), new Vector2(screenWidth, 1f));
    }

    void CreateCollider(Vector2 position, Vector2 size)
    {
        GameObject edge = new GameObject("EdgeCollider");
        edge.transform.position = position;

        BoxCollider2D collider = edge.AddComponent<BoxCollider2D>();
        collider.size = size;

        edge.transform.parent = transform; // Optional: Set as child of the current object for organization
    }
}
