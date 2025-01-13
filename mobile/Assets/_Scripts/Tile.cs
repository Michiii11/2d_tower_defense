using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _wall;

    public bool IsOccupied { get; private set; }

    private Rigidbody2D rb;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    private void Awake()
    {
        gameObject.tag = "Tile"; // Set the tag to "Tile"
        
        // Add or get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0; // Disable gravity
        rb.bodyType = RigidbodyType2D.Kinematic; // Use Dynamic for physics-based movement

        // Add a Collider2D if missing
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>(); // Adjust to fit your object
        }

        rb.tag = "Tile"; // Set the tag to "Tile"

        transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Ensure the correct Z position
    }

    // Method to place a block on the tile
    public void PlaceBlock()
    {
        IsOccupied = true;
        gameObject.tag = "Block"; // Change the tag to "Block"
        _wall.SetActive(true);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1); // Set Z position to -1
        // Additional logic to visually indicate the block
    }
 
    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseDown(){
        PlaceBlock();
    }
}

